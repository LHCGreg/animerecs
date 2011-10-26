using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeCompatibility;
using AnimeRecs.Common;

namespace AnimeRecs.Models
{
    public class RecommendorCacheRecommendationFinder : IRecommendationFinder
    {
        private IRecommendorCache m_recommendorCache;
        private bool m_disposeCache;

        public RecommendorCacheRecommendationFinder(IRecommendorCache recommendorCache, bool disposeCache = true)
        {
            m_recommendorCache = recommendorCache;
            m_disposeCache = disposeCache;
        }

        public RecommendationResults GetRecommendations(ICollection<MyAnimeListEntry> animeList)
        {
            const int recommendedPercentile = 30;
            const int dislikedPercentile = 30;
            
            RecommendationResults results = new RecommendationResults();
            List<Tuple<RecommendorJson, OneWayCompatibilityResults>> compatibilityScores = new List<Tuple<RecommendorJson, OneWayCompatibilityResults>>();
            
            CompatibilityCalculator calculator = new CompatibilityCalculator();
            PercentileGoodOkBadFilter goodOkBadFilter = new PercentileGoodOkBadFilter()
            {
                RecommendedPercentile = recommendedPercentile,
                DislikedPercentile = dislikedPercentile
            };

            CalculatorUserParams user = new CalculatorUserParams()
            {
                AnimeList = animeList,
                DislikedPenalty = -1,
                RecommendedBonus = 1,
                GoodOkBadFilter = goodOkBadFilter
            };

            GoodOkBadAnime usersFilteredAnime = goodOkBadFilter.GetGoodOkBadAnime(animeList);
            results.Disliked = usersFilteredAnime.BadAnime.Select(anime => ((MyAnimeListEntry)(anime)).ToAnimeJson()).ToList();
            results.Liked = usersFilteredAnime.GoodAnime.Select(anime => ((MyAnimeListEntry)(anime)).ToAnimeJson()).ToList();
            results.Ok = usersFilteredAnime.OkAnime.Select(anime => ((MyAnimeListEntry)(anime)).ToAnimeJson()).ToList();
            results.OkCutoff = usersFilteredAnime.OkCutoff;
            results.RecommendedCutoff = usersFilteredAnime.GoodCutoff;
  
            foreach (RecommendorJson recommendor in m_recommendorCache.GetRecommendors())
            {
                List<IAnimeListEntry> recommendorMalList = new List<IAnimeListEntry>(
                    from animeJson in recommendor.Recommendations
                    select new MyAnimeListEntry()
                    {
                        Id = animeJson.MalId,
                        Name = animeJson.Name,
                        Status = CompletionStatus.Completed // XXX: We don't really know if the recommendor completed it, but this is so that the calculator will take this anime into account
                    });

                OneWayCompatibilityResults compatibilityResults = calculator.GetOneWayCompatibilityScore(
                recommendedAnime: recommendorMalList, recommendee: user);
                compatibilityScores.Add(new Tuple<RecommendorJson, OneWayCompatibilityResults>(recommendor, compatibilityResults));
            }

            const int minimumRecsSeen = 8;
            const int minimumRecsNotSeen = 1;
            
            List<Tuple<RecommendorJson, OneWayCompatibilityResults>> sortedPrunedCompatibilityScores =
                compatibilityScores
                // Only count recommendors if the user has seen at least 8 of the animes that the recommendor recommends.
                // Less than that and the compatibility rating may not be very accurate.
                .Where(recommendorAndResults => recommendorAndResults.Item2.RecommendedAnimeInCommon.Count >= minimumRecsSeen)

                // Only count recommendors if there is at least 1 recommendation that the user has not seen yet.
                // Otherwise the recommendations are useless or maybe the recommendor is the same person!
                .Where(recommendorAndResults => recommendorAndResults.Item2.RecommendedAnimeNotInCommon.Count >= minimumRecsNotSeen)
                .OrderByDescending(recommendorAndResults => recommendorAndResults.Item2.NormalizedCompatibilityScore)
                .ToList();

            results.BestMatches = new List<RecommendorMatch>();
            int maxRecommendorsToTake = 3;
            for (int i = 0; i < maxRecommendorsToTake && i < sortedPrunedCompatibilityScores.Count; i++)
            {
                RecommendorJson recommendor = sortedPrunedCompatibilityScores[i].Item1;
                OneWayCompatibilityResults compatResults = sortedPrunedCompatibilityScores[i].Item2;
                var match = new RecommendorMatch()
                {
                    CompatibilityRating = new decimal(Math.Round(compatResults.NormalizedCompatibilityScore * 100, 2)),
                    Recommendor = recommendor
                };

                results.BestMatches.Add(match);
            }

            return results;
        }

        public void Dispose()
        {
            if (m_disposeCache)
            {
                m_recommendorCache.Dispose();
            }
        }
    }
}