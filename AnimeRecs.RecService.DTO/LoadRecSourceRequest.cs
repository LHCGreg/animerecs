using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using AnimeRecs.RecService.DTO.JsonConverters;

namespace AnimeRecs.RecService.DTO
{
    [JsonConverter(typeof(LoadRecSourceRequestJsonConverter))]
    public abstract class LoadRecSourceRequest
    {
        /// <summary>
        /// Name to give the loaded rec source. Other operations will use this name to refer to it. The name is not case-sensitive.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If false and there is already a rec source with the given name loaded, it will be an error.
        /// </summary>
        public bool ReplaceExisting { get; set; }

        /// <summary>
        /// Type of rec source.
        /// </summary>
        public string Type { get; set; }

        public LoadRecSourceRequest()
        {
            ;
        }

        public LoadRecSourceRequest(string name, bool replaceExisting, string type)
        {
            Name = name;
            ReplaceExisting = replaceExisting;
            Type = type;
        }

        // Used for dynamically setting params based on configuration
        public abstract RecSourceParams GetParams();
    }

    // No [JsonClass], preloading generic classes needs to be handled specially
    public class LoadRecSourceRequest<TRecSourceParams> : LoadRecSourceRequest
        where TRecSourceParams : RecSourceParams, new()
    {
        /// <summary>
        /// Parameters specific to the type of rec source.
        /// </summary>
        public TRecSourceParams Params { get; set; }

        public LoadRecSourceRequest()
        {
            Params = new TRecSourceParams();
        }

        public LoadRecSourceRequest(string name, string type, bool replaceExisting, TRecSourceParams parameters)
            : base(name: name, replaceExisting: replaceExisting, type: type)
        {
            Params = parameters;
        }

        public override RecSourceParams GetParams()
        {
            return Params;
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