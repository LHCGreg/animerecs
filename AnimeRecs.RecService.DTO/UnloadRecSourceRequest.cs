using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class UnloadRecSourceRequest
    {
        public string Name { get; set; }

        public UnloadRecSourceRequest()
        {
            ;
        }

        public UnloadRecSourceRequest(string name)
        {
            Name = name;
        }
    }
}
