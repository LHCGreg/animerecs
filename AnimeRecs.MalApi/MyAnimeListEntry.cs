using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.MalApi
{
    public class MyAnimeListEntry : IEquatable<MyAnimeListEntry>
    {
        public decimal? Score { get; set; }
        public CompletionStatus Status { get; set; }
        public int NumEpisodesWatched { get; set; }
        public DateTime? MyStartDate { get; set; }
        public DateTime? MyFinishDate { get; set; }
        public DateTime MyLastUpdate { get; set; }
        public MalAnimeInfoFromUserLookup AnimeInfo { get; set; }
        public ICollection<string> Tags { get; set; }

        public MyAnimeListEntry()
        {
            AnimeInfo = new MalAnimeInfoFromUserLookup();
        }

        public MyAnimeListEntry(decimal? score, CompletionStatus status, int numEpisodesWatched, DateTime? myStartDate,
            DateTime? myFinishDate, DateTime myLastUpdate, MalAnimeInfoFromUserLookup animeInfo, ICollection<string> tags)
        {
            Score = score;
            Status = status;
            NumEpisodesWatched = numEpisodesWatched;
            MyStartDate = myStartDate;
            MyFinishDate = myFinishDate;
            MyLastUpdate = myLastUpdate;
            AnimeInfo = animeInfo;
            Tags = tags;
        }

        public bool Equals(MyAnimeListEntry other)
        {
            if (other == null) return false;
            return this.AnimeInfo.AnimeId == other.AnimeInfo.AnimeId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MyAnimeListEntry);
        }

        public override int GetHashCode()
        {
            return AnimeInfo.AnimeId.GetHashCode();
        }

        public override string ToString()
        {
            return AnimeInfo.Title;
        }
    }
}

/*
 Copyright 2011 Greg Najda

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