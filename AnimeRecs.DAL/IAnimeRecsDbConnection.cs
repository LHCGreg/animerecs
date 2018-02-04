using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.DAL
{
    public interface IAnimeRecsDbConnection : IDisposable
    {
        Task OpenAsync(CancellationToken cancellationToken);
        Task<IDictionary<int, ICollection<streaming_service_anime_map>>> GetStreamsAsync(IEnumerable<int> malAnimeIds, CancellationToken cancellationToken);
    }

    public static class AnimeRecsDbConnectionExtensions
    {
        public static Task OpenAsync(this IAnimeRecsDbConnection conn)
        {
            return conn.OpenAsync(CancellationToken.None);
        }

        public static Task<IDictionary<int, ICollection<streaming_service_anime_map>>> GetStreamsAsync(this IAnimeRecsDbConnection conn, IEnumerable<int> malAnimeIds)
        {
            return conn.GetStreamsAsync(malAnimeIds, CancellationToken.None);
        }
    }
}
