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

        public const int DefaultMinimumRecsNotSeen = 1;
        public const int DefaultMaximumRecommendorsToReturn = 3;

        private int m_minimumRecsNotSeen = DefaultMinimumRecsNotSeen;
        public int MinimumRecsNotSeen { get { return m_minimumRecsNotSeen; } set { m_minimumRecsNotSeen = value; } }

        private int m_maximumRecommendorsToReturn = DefaultMaximumRecommendorsToReturn;
        public int MaximumRecommendorsToReturn { get { return m_maximumRecommendorsToReturn; } set { m_maximumRecommendorsToReturn = value; } }

        public RecommendorCacheRecommendationFinder(IRecommendorCache recommendorCache, bool disposeCache = true)
        {
            m_recommendorCache = recommendorCache;
            m_disposeCache = disposeCache;
        }

        public RecommendationResults GetRecommendations(ICollection<MyAnimeListEntry> animeList, IGoodOkBadFilter goodOkBadFilter)
        {
            RecommendationResults results = new RecommendationResults();
            List<Tuple<RecommendorJson, RecommendorCompatibility>> recommendorResults = new List<Tuple<RecommendorJson, RecommendorCompatibility>>();

            GoodOkBadAnime usersFilteredAnime = goodOkBadFilter.GetGoodOkBadAnime(animeList);

            results.NotLikedByMalId = new Dictionary<int, RecommendedAnimeJson>();
            IEnumerable<RecommendedAnimeJson> dislikedAnimeJson = usersFilteredAnime.BadAnime.Select(anime => ((MyAnimeListEntry)(anime)).ToAnimeJson());
            IEnumerable<RecommendedAnimeJson> okAnimeJson = usersFilteredAnime.OkAnime.Select(anime => ((MyAnimeListEntry)(anime)).ToAnimeJson());
            foreach (RecommendedAnimeJson anime in dislikedAnimeJson)
            {
                results.NotLikedByMalId[anime.MalId] = anime;
            }
            foreach (RecommendedAnimeJson anime in okAnimeJson)
            {
                results.NotLikedByMalId[anime.MalId] = anime;
            }

            results.LikedByMalId = new Dictionary<int, RecommendedAnimeJson>();
            IEnumerable<RecommendedAnimeJson> likedAnimeJson = usersFilteredAnime.GoodAnime.Select(anime => ((MyAnimeListEntry)(anime)).ToAnimeJson());
            foreach (RecommendedAnimeJson anime in likedAnimeJson)
            {
                results.LikedByMalId[anime.MalId] = anime;
            }

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

                //OneWayCompatibilityResults compatibilityResults = calculator.GetOneWayCompatibilityScore(
                //recommendedAnime: recommendorMalList, recommendee: user);
                //compatibilityScores.Add(new Tuple<RecommendorJson, OneWayCompatibilityResults>(recommendor, compatibilityResults));

                List<IAnimeListEntry> recsLiked = recommendorMalList.Where(anime => usersFilteredAnime.GoodAnime.Contains(anime)).ToList();
                List<IAnimeListEntry> recsNotLiked = recommendorMalList.Where(anime => usersFilteredAnime.OkAnime.Contains(anime) || usersFilteredAnime.BadAnime.Contains(anime)).ToList();

                if (recsLiked.Count == 0 && recsNotLiked.Count == 0)
                {
                    continue; // Skip recommendors user has nothing in common with
                }

                //RecommendorCompatibilityJson thisRecommendorResults = new RecommendorCompatibilityJson()
                //{
                //    //NumRecommendations = recommendorMalList.Count,
                //    //NumRecommendationsLiked = recsLiked.Count
                //    RecommendedAnime = recommendorMalList,
                //    RecommendedAnimeInCommon = recommendorMalList.Where(anime => usersFilteredAnime.GoodAnime.Contains(anime)
                //        || usersFilteredAnime.OkAnime.Contains(anime) || usersFilteredAnime.BadAnime.Contains(anime)).ToList(),
                //    RecommendedAnimeNotInCommon = recommendorMalList.Where(anime => !usersFilteredAnime.GoodAnime.Contains(anime)
                //        && !usersFilteredAnime.OkAnime.Contains(anime) && !usersFilteredAnime.BadAnime.Contains(anime)).ToList(),
                //    RecommendedAnimeLiked = recommendorMalList.Where(anime => usersFilteredAnime.GoodAnime.Contains(anime)).ToList(),
                //    RecommendedAnimeNotliked = recommendorMalList.Where(anime => usersFilteredAnime.OkAnime.Contains(anime)
                //        || usersFilteredAnime.BadAnime.Contains(anime)).ToList()
                //};

                if (recommendor.Name == "Bass00" || recommendor.Name == "auratrice")
                {
                    int blah = 3;
                }

                RecommendorCompatibility thisRecommendorResults = new RecommendorCompatibility(
                    recommendedAnime: recommendorMalList,
                    recommendedAnimeLiked: recommendorMalList.Where(anime => usersFilteredAnime.GoodAnime.Contains(anime)).ToList(),
                    recommendedAnimeInCommon: recommendorMalList.Where(anime => usersFilteredAnime.GoodAnime.Contains(anime)
                        || usersFilteredAnime.OkAnime.Contains(anime) || usersFilteredAnime.BadAnime.Contains(anime)).ToList());

                recommendorResults.Add(new Tuple<RecommendorJson, RecommendorCompatibility>(recommendor, thisRecommendorResults));
            }
            
            //List<Tuple<RecommendorJson, OneWayCompatibilityResults>> sortedPrunedCompatibilityScores =
            //    compatibilityScores
            //    // Only count recommendors if the user has seen at least X of the animes that the recommendor recommends.
            //    // Less than that and the compatibility rating may not be very accurate.
            //    .Where(recommendorAndResults => recommendorAndResults.Item2.RecommendedAnimeInCommon.Count >= MinimumRecsSeen)

            //    // Only count recommendors if there is at least X recommendation that the user has not seen yet.
            //    // Otherwise the recommendations are useless or maybe the recommendor is the same person!
            //    .Where(recommendorAndResults => recommendorAndResults.Item2.RecommendedAnimeNotInCommon.Count >= MinimumRecsNotSeen)
            //    .OrderByDescending(recommendorAndResults => recommendorAndResults.Item2.NormalizedCompatibilityScore)
            //    .ToList();

            List<Tuple<RecommendorJson, RecommendorCompatibility>> sortedPrunedCompatibilityScores =
                recommendorResults
                .Where(recommendorAndResults => recommendorAndResults.Item2.RecommendedAnimeNotInCommon.Count >= MinimumRecsNotSeen)
                .OrderByDescending(recommendorAndResults => recommendorAndResults.Item2.Compatibility80PercentConfidenceInterval.Item1)
                .ToList();

            results.BestMatches = new List<RecommendorMatch>();
            for (int i = 0; i < MaximumRecommendorsToReturn && i < sortedPrunedCompatibilityScores.Count; i++)
            {
                RecommendorJson recommendor = sortedPrunedCompatibilityScores[i].Item1;
                //OneWayCompatibilityResults compatResults = sortedPrunedCompatibilityScores[i].Item2;
                //var match = new RecommendorMatch()
                //{
                //    CompatibilityRating = new decimal(Math.Round(compatResults.NormalizedCompatibilityScore * 100, 2)),
                //    Recommendor = recommendor
                //};
                var match = new RecommendorMatch()
                {
                    //CompatibilityRating = new decimal(Math.Round(sortedPrunedCompatibilityScores[i].Item2.Compatibility90PercentConfidenceInterval.Item1 * 100, 2)),
                    PercentLiked = new decimal(Math.Round(sortedPrunedCompatibilityScores[i].Item2.FractionLiked * 100, 2)),
                    LowerBound = new decimal(Math.Round(sortedPrunedCompatibilityScores[i].Item2.Compatibility80PercentConfidenceInterval.Item1 * 100, 2)),
                    UpperBound = new decimal(Math.Round(sortedPrunedCompatibilityScores[i].Item2.Compatibility80PercentConfidenceInterval.Item2 * 100, 2)),
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