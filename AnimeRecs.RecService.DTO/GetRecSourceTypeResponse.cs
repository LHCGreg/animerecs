using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    [ResponseJsonClass]
    public class GetRecSourceTypeResponse
    {
        public string RecSourceType { get; set; }

        public GetRecSourceTypeResponse()
        {
            ;
        }

        public GetRecSourceTypeResponse(string recSourceType)
        {
            RecSourceType = recSourceType;
        }
    }
}
