using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeCompatibility;

namespace AnimeRecs.Models
{
    public class RecommendorCacheRecommendationFinder : IRecommendationFinder
    {
        private IRecommendorCacheFactory m_cacheFactory;

        public RecommendorCacheRecommendationFinder(IRecommendorCacheFactory cacheFactory)
        {
            m_cacheFactory = cacheFactory;
        }

        public RecommendationResults GetRecommendations(ICollection<MyAnimeListEntry> animeList)
        {
            RecommendationResults results = new RecommendationResults();
            List<Tuple<Recommendor, OneWayCompatibilityResults>> compatibilityScores = new List<Tuple<Recommendor, OneWayCompatibilityResults>>();
            
            using (IRecommendorCache recommendorCache = m_cacheFactory.GetCache())
            {
                CompatibilityCalculator calculator = new CompatibilityCalculator();
                CalculatorUserParams user = new CalculatorUserParams()
                {
                    CompletedAnime = animeList,
                    DislikedPenalty = -1,
                    DislikedPercentile = 25,
                    RecommendedBonus = 1,
                    RecommendedPercentile = 25
                };
  
                foreach (Recommendor recommendor in recommendorCache.Recommendors)
                {
                    List<IAnimeListEntry> malList = new List<IAnimeListEntry>();
                    foreach (AnimeJson animeJson in recommendor.Recommendations)
                    {
                        if (animeJson.MalId.HasValue)
                        {
                            malList.Add(new MyAnimeListEntry() { Id = animeJson.MalId.Value, Name = animeJson.Name, Status = CompletionStatus.Completed });
                        }
                    }

                    OneWayCompatibilityResults compatibilityResults = calculator.GetOneWayCompatibilityScore(malList, user);
                    compatibilityScores.Add(new Tuple<Recommendor, OneWayCompatibilityResults>(recommendor, compatibilityResults));

                    if (results.Disliked == null)
                    {
                        results.Disliked = compatibilityResults.RecommendeeGoodBadOkAnime.BadAnime.Select(animeListEntry => ((MyAnimeListEntry)(animeListEntry)).ToAnimeJson()).ToList();
                        results.Liked = compatibilityResults.RecommendeeGoodBadOkAnime.GoodAnime.Select(animeListEntry => ((MyAnimeListEntry)(animeListEntry)).ToAnimeJson()).ToList();
                        results.Ok = compatibilityResults.RecommendeeGoodBadOkAnime.OkAnime.Select(animeListEntry => ((MyAnimeListEntry)(animeListEntry)).ToAnimeJson()).ToList();
                        results.OkCutoff = compatibilityResults.RecommendeeGoodBadOkAnime.OkCutoff;
                        results.RecommendedCutoff = compatibilityResults.RecommendeeGoodBadOkAnime.GoodCutoff;
                    }
                }
            }

            compatibilityScores.Sort(
                (recommendorAndResults1, recommendorAndResults2) =>
                    -(recommendorAndResults1.Item2.NormalizedCompatibilityScore.CompareTo(recommendorAndResults2.Item2.NormalizedCompatibilityScore)));

            results.BestMatches = new List<RecommendorMatch>();
            int maxRecommendorsToTake = 3;
            for (int i = 0; i < maxRecommendorsToTake && i < compatibilityScores.Count; i++)
            {
                var match = new RecommendorMatch()
                {
                    CompatibilityRating = new decimal(Math.Round(compatibilityScores[i].Item2.NormalizedCompatibilityScore, 2)),
                    Recommendor = compatibilityScores[i].Item1
                };

                results.BestMatches.Add(match);
            }

            return results;
        }
    }
}