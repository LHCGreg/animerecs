using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MalApi;

namespace AnimeRecs.RecEngine.MAL
{
    public struct MalListEntry
    {
        // 255 means null rating
        private byte m_rating;

        public byte? Rating
        {
            get
            {
                if (m_rating == 255)
                {
                    return null;
                }
                else
                {
                    return m_rating;
                }
            }
            private set
            {
                if (value == null)
                {
                    m_rating = 255;
                }
                else
                {
                    m_rating = value.Value;
                }
            }
        }

        private byte m_status;
        public CompletionStatus Status
        {
            get
            {
                return (CompletionStatus)m_status;
            }
            private set
            {
                m_status = (byte)value;
            }
        }

        public short NumEpisodesWatched { get; private set; }

        public MalListEntry(byte? rating, CompletionStatus status, short numEpisodesWatched)
            : this()
        {
            Rating = rating;
            Status = status;
            NumEpisodesWatched = numEpisodesWatched;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", Rating.HasValue ? Rating.Value.ToString() : "?", Status);
        }
    }
}
