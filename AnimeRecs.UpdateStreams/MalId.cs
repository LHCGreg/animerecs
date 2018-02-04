using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace AnimeRecs.UpdateStreams
{
    /// <summary>
    /// Not specified means a blank in the csv. Specified but null MalId means n/a.
    /// </summary>
    struct MalId
    {
        public bool Specified { get; private set; }
        public int? MalAnimeId { get; private set; }

        public MalId(int? malAnimeId, bool specified)
            : this()
        {
            MalAnimeId = malAnimeId;
            Specified = specified;
        }

        public override string ToString()
        {
            if (MalAnimeId != null)
            {
                return MalAnimeId.Value.ToString(CultureInfo.InvariantCulture);
            }
            else if (Specified)
            {
                return "n/a";
            }
            else
            {
                return "";
            }
        }
    }
}
