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

        public const int defaultMinimumRecsSeen = 8;
        public const int defaultMinimumRecsNotSeen = 1;
        public const int defaultMaximumRecommendorsToReturn = 3;

        private int m_minimumRecsSeen = defaultMinimumRecsSeen;
        public int MinimumRecsSeen { get { return m_minimumRecsSeen; } set { m_minimumRecsSeen = value; } }

        private int m_minimumRecsNotSeen = defaultMinimumRecsNotSeen;
        public int MinimumRecsNotSeen { get { return m_minimumRecsNotSeen; } set { m_minimumRecsNotSeen = value; } }

        private int m_maximumRecommendorsToReturn = defaultMaximumRecommendorsToReturn;
        public int MaximumRecommendorsToReturn { get { return m_maximumRecommendorsToReturn; } set { m_maximumRecommendorsToReturn = value; } }

        public RecommendorCacheRecommendationFinder(IRecommendorCache recommendorCache, bool disposeCache = true)
        {
            m_recommendorCache = recommendorCache;
            m_disposeCache = disposeCache;
        }

        public RecommendationResults GetRecommendations(ICollection<MyAnimeListEntry> animeList, IGoodOkBadFilter goodOkBadFilter)
        {
            RecommendationResults results = new RecommendationResults();
            List<Tuple<RecommendorJson, OneWayCompatibilityResults>> compatibilityScores = new List<Tuple<RecommendorJson, OneWayCompatibilityResults>>();
            
            CompatibilityCalculator calculator = new CompatibilityCalculator();

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
            
            List<Tuple<RecommendorJson, OneWayCompatibilityResults>> sortedPrunedCompatibilityScores =
                compatibilityScores
                // Only count recommendors if the user has seen at least 8 of the animes that the recommendor recommends.
                // Less than that and the compatibility rating may not be very accurate.
                .Where(recommendorAndResults => recommendorAndResults.Item2.RecommendedAnimeInCommon.Count >= MinimumRecsSeen)

                // Only count recommendors if there is at least 1 recommendation that the user has not seen yet.
                // Otherwise the recommendations are useless or maybe the recommendor is the same person!
                .Where(recommendorAndResults => recommendorAndResults.Item2.RecommendedAnimeNotInCommon.Count >= MinimumRecsNotSeen)
                .OrderByDescending(recommendorAndResults => recommendorAndResults.Item2.NormalizedCompatibilityScore)
                .ToList();

            results.BestMatches = new List<RecommendorMatch>();
            for (int i = 0; i < MaximumRecommendorsToReturn && i < sortedPrunedCompatibilityScores.Count; i++)
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