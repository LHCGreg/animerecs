using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.MalApi;

namespace AnimeRecs.RecService.DTO
{
    public class MalAnimeRecsExtraResponseData
    {
        public IList<MalAnimeRecsRecommender> Recommenders { get; set; }

        public MalAnimeRecsExtraResponseData()
        {
            ;
        }

        public MalAnimeRecsExtraResponseData(IList<MalAnimeRecsRecommender> recommenders)
        {
            Recommenders = recommenders;
        }
    }

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

    public class MalAnimeRecsRecommenderRecommendation
    {
        public int MalAnimeId { get; set; }
        public bool? Liked { get; set; }
        public decimal? RecommenderScore { get; set; }
        public double AverageScore { get; set; }

        public MalAnimeRecsRecommenderRecommendation()
        {
            ;
        }

        public MalAnimeRecsRecommenderRecommendation(int malAnimeId, bool? liked, decimal? recommenderScore, double averageScore)
        {
            MalAnimeId = malAnimeId;
            Liked = liked;
            RecommenderScore = recommenderScore;
            AverageScore = averageScore;
        }
    }
}
