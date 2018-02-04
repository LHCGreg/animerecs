using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class MalPositiveFeedbackExtraResponseData
    {
        public decimal TargetScoreUsed { get; set; }

        public MalPositiveFeedbackExtraResponseData()
        {
            ;
        }

        public MalPositiveFeedbackExtraResponseData(decimal targetScoreUsed)
        {
            TargetScoreUsed = targetScoreUsed;
        }
    }
}
