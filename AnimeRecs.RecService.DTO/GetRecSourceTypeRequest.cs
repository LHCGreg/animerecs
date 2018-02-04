using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class GetRecSourceTypeRequest
    {
        public string RecSourceName { get; set; }

        public GetRecSourceTypeRequest()
        {
            ;
        }

        public GetRecSourceTypeRequest(string recSourceName)
        {
            RecSourceName = recSourceName;
        }
    }
}
