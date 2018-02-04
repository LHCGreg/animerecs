using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class MalListForUser
    {
        public IList<MalListEntry> Entries { get; set; }

        public MalListForUser()
        {
            ;
        }

        public MalListForUser(IList<MalListEntry> entries)
        {
            Entries = entries;
        }

        public override string ToString()
        {
            return string.Format("{0} entries", Entries.Count);
        }
    }
}
