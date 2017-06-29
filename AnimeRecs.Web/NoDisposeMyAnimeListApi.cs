using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MalApi;

namespace AnimeRecs.Web
{
    /// <summary>
    /// Wraps another IMyAnimeListApi and does not dispose of the other api.
    /// </summary>
    class NoDisposeMyAnimeListApi : IMyAnimeListApi
    {
        private IMyAnimeListApi _underlyingApi;

        public NoDisposeMyAnimeListApi(IMyAnimeListApi underlyingApi)
        {
            _underlyingApi = underlyingApi;
        }

        public MalUserLookupResults GetAnimeListForUser(string user)
        {
            return _underlyingApi.GetAnimeListForUser(user);
        }

        public Task<MalUserLookupResults> GetAnimeListForUserAsync(string user)
        {
            return _underlyingApi.GetAnimeListForUserAsync(user);
        }

        public Task<MalUserLookupResults> GetAnimeListForUserAsync(string user, CancellationToken cancellationToken)
        {
            return _underlyingApi.GetAnimeListForUserAsync(user, cancellationToken);
        }

        public RecentUsersResults GetRecentOnlineUsers()
        {
            return _underlyingApi.GetRecentOnlineUsers();
        }

        public Task<RecentUsersResults> GetRecentOnlineUsersAsync()
        {
            return _underlyingApi.GetRecentOnlineUsersAsync();
        }

        public Task<RecentUsersResults> GetRecentOnlineUsersAsync(CancellationToken cancellationToken)
        {
            return _underlyingApi.GetRecentOnlineUsersAsync(cancellationToken);
        }

        public AnimeDetailsResults GetAnimeDetails(int animeId)
        {
            return _underlyingApi.GetAnimeDetails(animeId);
        }

        public Task<AnimeDetailsResults> GetAnimeDetailsAsync(int animeId)
        {
            return _underlyingApi.GetAnimeDetailsAsync(animeId);
        }

        public Task<AnimeDetailsResults> GetAnimeDetailsAsync(int animeId, CancellationToken cancellationToken)
        {
            return _underlyingApi.GetAnimeDetailsAsync(animeId, cancellationToken);
        }

        public void Dispose()
        {
            ;
        }
    }
}

// Copyright (C) 2017 Greg Najda
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