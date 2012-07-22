using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AnimeRecs.RecService.DTO.JsonConverters
{
    class OperationJsonConverter : JsonCreationConverter<Operation>
    {
        protected override Operation Create(Type objectType, JObject jObject)
        {
            string opName = jObject.Value<string>("OpName");

            if (opName.Equals(OpNames.GetMalRecs, StringComparison.OrdinalIgnoreCase))
                return new Operation<GetMalRecsRequest>();
            else if (opName.Equals(OpNames.GetRecSourceType, StringComparison.OrdinalIgnoreCase))
                return new Operation<GetRecSourceTypeRequest>();
            else if (opName.Equals(OpNames.LoadRecSource, StringComparison.OrdinalIgnoreCase))
                return new Operation<LoadRecSourceRequest>();
            else if (opName.Equals(OpNames.Ping, StringComparison.OrdinalIgnoreCase))
                return new Operation<PingRequest>();
            else if (opName.Equals(OpNames.ReloadTrainingData, StringComparison.OrdinalIgnoreCase))
                return new Operation();
            else if (opName.Equals(OpNames.UnloadRecSource, StringComparison.OrdinalIgnoreCase))
                return new Operation<UnloadRecSourceRequest>();
            else throw new Newtonsoft.Json.JsonException(string.Format("Operation {0} not recognized.", opName));
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