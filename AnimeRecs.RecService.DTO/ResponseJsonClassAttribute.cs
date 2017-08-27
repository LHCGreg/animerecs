using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    /// <summary>
    /// Used to mark a class as used as the T in Response&lt;T&gt;. This can be used for preserializing all JSON classes
    /// in long-running processes like servers to speed up the first use of a class for serializing/deserializing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class ResponseJsonClassAttribute : Attribute
    {
        public ResponseJsonClassAttribute()
        {

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
