using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using AnimeRecs.RecService.DTO.JsonConverters;

namespace AnimeRecs.RecService.DTO
{
    [JsonConverter(typeof(OperationJsonConverter))]
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

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.DTO.
//
// AnimeRecs.RecService.DTO is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.DTO is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.DTO.  If not, see <http://www.gnu.org/licenses/>.