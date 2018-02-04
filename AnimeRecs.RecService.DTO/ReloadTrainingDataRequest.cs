using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class ReloadTrainingDataRequest
    {
        public string Mode { get; set; }
        public bool Finalize { get; set; }

        public ReloadTrainingDataRequest()
        {
            Mode = ReloadBehavior.HighAvailability.ToString();
            Finalize = false;
        }

        public ReloadTrainingDataRequest(ReloadBehavior mode, bool finalize)
        {
            Mode = mode.ToString();
            Finalize = finalize;
        }
    }
}
