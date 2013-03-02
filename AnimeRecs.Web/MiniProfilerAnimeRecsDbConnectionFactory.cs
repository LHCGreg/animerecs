using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.DAL;
using System.Data;
using StackExchange.Profiling.Data;
using StackExchange.Profiling;
using System.Data.Common;

namespace AnimeRecs.Web
{
    public class MiniProfilerAnimeRecsDbConnectionFactory : IAnimeRecsDbConnectionFactory
    {
        private IAnimeRecsDbConnectionFactory m_wrappedFactory;

        public MiniProfilerAnimeRecsDbConnectionFactory(IAnimeRecsDbConnectionFactory wrappedFactory)
        {
            m_wrappedFactory = wrappedFactory;
        }
    
        public IAnimeRecsDbConnection GetConnection()
        {
            IAnimeRecsDbConnection animeRecsConn = m_wrappedFactory.GetConnection();
            IDbConnection conn = animeRecsConn.Conn;
            DbConnection casted = (DbConnection) conn; // One little cast couldn't hurt, right?
            ProfiledDbConnection wrappedConn = new ProfiledDbConnection(casted, MiniProfiler.Current);
            return new AnimeRecsDbConnection(wrappedConn);
        }
    }
}

// Copyright (C) 2013 Greg Najda
//
// This file is part of AnimeRecs.Web.
//
// AnimeRecs.Web is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.Web is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.Web.  If not, see <http://www.gnu.org/licenses/>.