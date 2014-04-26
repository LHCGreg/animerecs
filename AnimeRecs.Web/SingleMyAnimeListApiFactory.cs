using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MalApi;

namespace AnimeRecs.Web
{
    /// <summary>
    /// Keeps a single IMyAnimeListApi instance to give out to callers. The instance is not disposed when callers call Dispose() on the
    /// object they get. The instance is only disposed when Dispose() is called on this factory.
    /// </summary>
    public class SingleMyAnimeListApiFactory : IMyAnimeListApiFactory
    {
        private IMyAnimeListApi m_api;

        public SingleMyAnimeListApiFactory(IMyAnimeListApi api)
        {
            m_api = api;
        }

        public IMyAnimeListApi GetMalApi()
        {
            return new NoDisposeMyAnimeListApi(m_api);
        }

        public void Dispose()
        {
            m_api.Dispose();
        }
    }
}

// Copyright (C) 2012 Greg Najda
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