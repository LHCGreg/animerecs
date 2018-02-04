using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class GetMalRecsRequest
    {
        public MalListForUser AnimeList { get; set; }
        public string RecSourceName { get; set; }
        public int NumRecsDesired { get; set; }

        // XXX: Move these to rec source-specific DTOs
        public decimal? TargetScore { get; set; }
        public decimal? TargetFraction { get; set; }

        public GetMalRecsRequest()
        {
            ;
        }

        public static GetMalRecsRequest CreateWithTargetScore(string recSourceName, int numRecsDesired, decimal targetScore, MalListForUser animeList)
        {
            return new GetMalRecsRequest()
            {
                RecSourceName = recSourceName,
                NumRecsDesired = numRecsDesired,
                TargetScore = targetScore,
                AnimeList = animeList
            };
        }

        public static GetMalRecsRequest CreateWithTargetFraction(string recSourceName, int numRecsDesired, decimal targetFraction, MalListForUser animeList)
        {
            return new GetMalRecsRequest()
            {
                RecSourceName = recSourceName,
                NumRecsDesired = numRecsDesired,
                TargetFraction = targetFraction,
                AnimeList = animeList
            };
        }
    }
}
