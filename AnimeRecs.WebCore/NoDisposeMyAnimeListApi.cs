using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MalApi;

namespace AnimeRecs.WebCore
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
