using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using AnimeRecs.RecService.DTO.JsonConverters;

namespace AnimeRecs.RecService.DTO
{
    [JsonConverter(typeof(OperationJsonConverter))]
    // No [JsonClass], preloading generic classes needs to be handled specially
    public class Operation
    {
        public string OpName { get; set; }

        public Operation()
        {
            ;
        }

        public Operation(string opName)
        {
            OpName = opName;
        }
    }

    // No [JsonClass], preloading generic classes needs to be handled specially
    public class Operation<TPayload> : Operation
    {
        private TPayload m_payload;
        private bool m_payloadSet = false;
        public TPayload Payload { get { return m_payload; } set { m_payload = value; m_payloadSet = true; } }
        public bool PayloadSet { get { return m_payloadSet; } }

        public Operation()
        {
            ;
        }

        public Operation(string opName, TPayload payload)
            : base(opName)
        {
            Payload = payload;
        }
    }
}
