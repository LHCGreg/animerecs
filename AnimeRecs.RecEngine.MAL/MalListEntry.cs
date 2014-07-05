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

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.MAL.
//
// AnimeRecs.RecEngine.MAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine.MAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.MAL.  If not, see <http://www.gnu.org/licenses/>.