using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.MalApi
{
    public class MalAnimeInfoFromUserLookup : IEquatable<MalAnimeInfoFromUserLookup>
    {
        public int AnimeId { get; set; }
        public string Title { get; set; }

        /// <summary>
        /// Could be something other than the enumerated values if MAL adds new types!
        /// </summary>
        public MalAnimeType Type { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as MalAnimeInfoFromUserLookup);
        }
        
        public bool Equals(MalAnimeInfoFromUserLookup other)
        {
            if (other == null) return false;
            return this.AnimeId == other.AnimeId;
        }

        public override int GetHashCode()
        {
            return AnimeId.GetHashCode();
        }

        public override string ToString()
        {
            return Title;
        }
    }
}

/*
 Copyright 2012 Greg Najda

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/