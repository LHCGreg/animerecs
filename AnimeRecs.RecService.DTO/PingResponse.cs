using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    [ResponseJsonClass]
    public class PingResponse
    {
        public string OriginalMessage { get; set; }
        public string ResponseMessage { get; set; }
    }
}
