using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.DAL
{
    /// <summary>
    /// Returns a new connection for each call.
    /// </summary>
    public class AnimeRecsDbConnectionFactory : IAnimeRecsDbConnectionFactory
    {
        private string m_connectionString;

        public AnimeRecsDbConnectionFactory(string pgConnectionString)
        {
            m_connectionString = pgConnectionString;
        }

        public IAnimeRecsDbConnection GetConnection()
        {
            return new AnimeRecsDbConnection(m_connectionString);
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.DAL.
//
// AnimeRecs.DAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.DAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.DAL.  If not, see <http://www.gnu.org/licenses/>.