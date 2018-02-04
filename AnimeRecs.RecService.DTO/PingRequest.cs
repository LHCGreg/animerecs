using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class PingRequest
    {
        public string PingMessage { get; set; }

        public PingRequest()
        {
            ;
        }

        public PingRequest(string pingMessage)
        {
            PingMessage = pingMessage;
        }
    }
}
