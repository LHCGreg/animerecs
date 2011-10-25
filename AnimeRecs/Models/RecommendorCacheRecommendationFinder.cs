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
            RecommendationResults results = new RecommendationResults();
            List<Tuple<RecommendorJson, OneWayCompatibilityResults>> compatibilityScores = new List<Tuple<RecommendorJson, OneWayCompatibilityResults>>();
            
            CompatibilityCalculator calculator = new CompatibilityCalculator();
            PercentileGoodOkBadFilter goodOkBadFilter = new PercentileGoodOkBadFilter() { RecommendedPercentile = 25, DislikedPercentile = 25 };
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

            compatibilityScores.Sort(
                (recommendorAndResults1, recommendorAndResults2) =>
                    -(recommendorAndResults1.Item2.NormalizedCompatibilityScore.CompareTo(recommendorAndResults2.Item2.NormalizedCompatibilityScore)));

            results.BestMatches = new List<RecommendorMatch>();
            int maxRecommendorsToTake = 3;
            for (int i = 0; i < maxRecommendorsToTake && i < compatibilityScores.Count; i++)
            {
                RecommendorJson recommendor = compatibilityScores[i].Item1;
                OneWayCompatibilityResults compatResults = compatibilityScores[i].Item2;
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