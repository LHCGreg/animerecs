using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class MalAnimeRecsExtraResponseData
    {
        public IList<MalAnimeRecsRecommender> Recommenders { get; set; }
        public decimal TargetScoreUsed { get; set; }

        public MalAnimeRecsExtraResponseData()
        {
            ;
        }

        public MalAnimeRecsExtraResponseData(IList<MalAnimeRecsRecommender> recommenders, decimal targetScoreUsed)
        {
            Recommenders = recommenders;
            TargetScoreUsed = targetScoreUsed;
        }
    }

    [JsonClass]
    public class MalAnimeRecsRecommender
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public IList<MalAnimeRecsRecommenderRecommendation> Recs { get; set; }
        public double? Compatibility { get; set; }
        public double? CompatibilityLowEndpoint { get; set; }
        public double? CompatibilityHighEndpoint { get; set; }

        public MalAnimeRecsRecommender()
        {
            ;
        }

        public MalAnimeRecsRecommender(int userId, string username, IList<MalAnimeRecsRecommenderRecommendation> recs,
            double? compatibility, double? compatibilityLowEndpoint, double? compatibilityHighEndpoint)
        {
            UserId = userId;
            Username = username;
            Recs = recs;
            Compatibility = compatibility;
            CompatibilityLowEndpoint = compatibilityLowEndpoint;
            CompatibilityHighEndpoint = compatibilityHighEndpoint;
        }
    }

    public enum AnimeRecsRecommendationJudgment
    {
        Liked,
        NotLiked,
        Inconclusive,
        NotInCommon
    }

    [JsonClass]
    public class MalAnimeRecsRecommenderRecommendation
    {
        public int MalAnimeId { get; set; }
        public AnimeRecsRecommendationJudgment Judgment { get; set; }
        public decimal? RecommenderScore { get; set; }
        public double AverageScore { get; set; }

        public MalAnimeRecsRecommenderRecommendation()
        {
            ;
        }

        public MalAnimeRecsRecommenderRecommendation(int malAnimeId, AnimeRecsRecommendationJudgment judgment, decimal? recommenderScore, double averageScore)
        {
            MalAnimeId = malAnimeId;
            Judgment = judgment;
            RecommenderScore = recommenderScore;
            AverageScore = averageScore;
        }
    }
}
