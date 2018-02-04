using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using AnimeRecs.RecService.DTO.JsonConverters;

namespace AnimeRecs.RecService.DTO
{
    [JsonConverter(typeof(GetMalRecsResponseJsonConverter))]
    // No [JsonClass], preloading generic classes needs to be handled specially
    public class GetMalRecsResponse
    {
        public string RecommendationType { get; set; }
        public IList<DTO.MalAnime> Animes { get; set; }

        public GetMalRecsResponse()
        {
            ;
        }

        public GetMalRecsResponse(string recommendationType, IList<DTO.MalAnime> animes)
        {
            RecommendationType = recommendationType;
            Animes = animes;
        }
    }

    // No [JsonClass], preloading generic classes needs to be handled specially
    public class GetMalRecsResponse<TRecommendation> : GetMalRecsResponse
        where TRecommendation : Recommendation
    {
        public IList<TRecommendation> Recommendations { get; set; }

        public GetMalRecsResponse()
        {
            ;
        }

        public GetMalRecsResponse(string recommendationType, IList<DTO.MalAnime> animes, IList<TRecommendation> recommendations)
            : base(recommendationType, animes)
        {
            Recommendations = recommendations;
        }
    }

    // No [JsonClass], preloading generic classes needs to be handled specially
    public class GetMalRecsResponse<TRecommendation, TData> : GetMalRecsResponse<TRecommendation>
        where TRecommendation : Recommendation
    {
        public TData Data { get; set; }

        public GetMalRecsResponse()
        {
            ;
        }

        public GetMalRecsResponse(string recommendationType, IList<DTO.MalAnime> animes, IList<TRecommendation> recommendations, TData data)
            : base(recommendationType, animes, recommendations)
        {
            Data = data;
        }
    }
}
