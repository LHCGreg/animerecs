using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
{
    class AnimeStreamInfo : IEquatable<AnimeStreamInfo>
    {
        public string AnimeName { get; private set; }
        public string Url { get; private set; }
        public StreamingService Service { get; private set; }

        public AnimeStreamInfo(string animeName, string url, StreamingService service)
        {
            AnimeName = animeName;
            Url = url;
            Service = service;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AnimeStreamInfo);
        }

        public bool Equals(AnimeStreamInfo other)
        {
            if (other == null) return false;
            return this.AnimeName == other.AnimeName && this.Url == other.Url && this.Service == other.Service;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 23;
                hash = hash * 17 + AnimeName.GetHashCode();
                hash = hash * 17 + Url.GetHashCode();
                hash = hash * 17 + Service.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return string.Format("AnimeName=\"{0}\" Url={1} Service={2}", AnimeName, Url, Service);
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.UpdateStreams
//
// AnimeRecs.UpdateStreams is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.UpdateStreams is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.UpdateStreams.  If not, see <http://www.gnu.org/licenses/>.
//
//  If you modify AnimeRecs.UpdateStreams, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.