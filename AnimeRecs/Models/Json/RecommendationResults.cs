using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.Common;
using System.Globalization;
using Newtonsoft.Json;

namespace AnimeRecs.Models
{
    public enum RecommendationStatus
    {
        // Put these in sort order
        Unwatched,
        Liked,
        NotLiked
    }

    public class RecommendationResults
    {
        public decimal? RecommendedCutoff { get; set; }

        [JsonIgnore]
        public IDictionary<int, RecommendedAnimeJson> LikedByMalId { get; set; }

        [JsonIgnore]
        public IDictionary<int, RecommendedAnimeJson> NotLikedByMalId { get; set; }

        public IEnumerable<RecommendedAnimeJson> Liked { get { return LikedByMalId.Values; } }
        public IEnumerable<RecommendedAnimeJson> NotLiked { get { return NotLikedByMalId.Values; } }

        public IList<RecommendorMatch> BestMatches { get; set; }

        public RecommendationStatus GetStatus(int malId)
        {
            if (LikedByMalId.ContainsKey(malId))
            {
                return RecommendationStatus.Liked;
            }
            else if (NotLikedByMalId.ContainsKey(malId))
            {
                return RecommendationStatus.NotLiked;
            }
            else
            {
                return RecommendationStatus.Unwatched;
            }
        }

        public static string GetRecommendationStatusString(RecommendationStatus status)
        {
            if (status == RecommendationStatus.Liked)
            {
                return "Liked";
            }
            else if (status == RecommendationStatus.NotLiked)
            {
                return "Not liked";
            }
            else
            {
                return "Unwatched";
            }
        }

        public string GetStatusString(int malId)
        {
            return GetRecommendationStatusString(GetStatus(malId));
        }

        public string GetMyScoreString(int malId)
        {
            decimal? myScore = GetMyScore(malId);
            if (myScore != null)
                return myScore.Value.ToString(CultureInfo.InvariantCulture);
            else
                return "-";
        }

        public decimal? GetMyScore(int malId)
        {
            if (LikedByMalId.ContainsKey(malId))
                return LikedByMalId[malId].Rating;
            else if (NotLikedByMalId.ContainsKey(malId))
                return NotLikedByMalId[malId].Rating;
            else
                return null;
        }
    }
}