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
        Ok,
        Disliked
    }

    public class RecommendationResults
    {
        public decimal? RecommendedCutoff { get; set; }
        public decimal? OkCutoff { get; set; }

        [JsonIgnore]
        public IDictionary<int, RecommendedAnimeJson> LikedByMalId { get; set; }

        [JsonIgnore]
        public IDictionary<int, RecommendedAnimeJson> OkByMalId { get; set; }

        [JsonIgnore]
        public IDictionary<int, RecommendedAnimeJson> DislikedByMalId { get; set; }

        public IEnumerable<RecommendedAnimeJson> Liked { get { return LikedByMalId.Values; } }
        public IEnumerable<RecommendedAnimeJson> Ok { get { return OkByMalId.Values; } }
        public IEnumerable<RecommendedAnimeJson> Disliked { get { return DislikedByMalId.Values; } }

        public IList<RecommendorMatch> BestMatches { get; set; }

        public RecommendationStatus GetStatus(int malId)
        {
            if (LikedByMalId.ContainsKey(malId))
            {
                return RecommendationStatus.Liked;
            }
            else if (OkByMalId.ContainsKey(malId))
            {
                return RecommendationStatus.Ok;
            }
            else if (DislikedByMalId.ContainsKey(malId))
            {
                return RecommendationStatus.Disliked;
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
            else if (status == RecommendationStatus.Ok)
            {
                return "Ok";
            }
            else if (status == RecommendationStatus.Disliked)
            {
                return "Disliked";
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
            else if (OkByMalId.ContainsKey(malId))
                return OkByMalId[malId].Rating;
            else if (DislikedByMalId.ContainsKey(malId))
                return DislikedByMalId[malId].Rating;
            else
                return null;
        }
    }
}