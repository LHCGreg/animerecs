using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AnimeRecs.RecService.DTO.JsonConverters
{
    class OperationJsonConverter : JsonCreationConverter<Operation>
    {
        protected override Operation Create(Type objectType, JObject jObject)
        {
            string opName = jObject.Value<string>("OpName");

            if (!OperationTypes.OperationFactories.TryGetValue(opName, out Func<Operation> opFactory))
            {
                throw new Newtonsoft.Json.JsonException(string.Format("Operation {0} not recognized.", opName));
            }

            return opFactory();
        }
    }
}

// Copyright (C) 2017 Greg Najda
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