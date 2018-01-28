using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.ClientLib;
using MalApi;

namespace AnimeRecs.WebCore.Models
{
    public class GetRecsViewModel
    {
        public MalRecResults<IEnumerable<IRecommendation>> Results { get; private set; }
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public MalUserLookupResults UserLookup { get; private set; }
        public IDictionary<int, MalListEntry> UserAnimeList { get; private set; }
        public IDictionary<int, MalListEntry> AnimeWithheld { get; private set; }
        public int MaximumRecommendationsToReturn { get; private set; }
        public int MaximumRecommendersToReturn { get; private set; }

        private IAnimeRecsDbConnectionFactory DbConnectionFactory { get; set; }

        /// <summary>
        /// You must call DeclareAnimeToBeDisplayed() to populate this.
        /// </summary>
        public IDictionary<int, ICollection<streaming_service_anime_map>> StreamsByAnime { get; set; }

        public GetRecsViewModel(MalRecResults<IEnumerable<IRecommendation>> results, int userId, string userName,
            MalUserLookupResults userLookup, IDictionary<int, MalListEntry> userAnimeList,
            int maximumRecommendationsToReturn, int maximumRecommendersToReturn, IDictionary<int, MalListEntry> animeWithheld, IAnimeRecsDbConnectionFactory dbConnectionFactory)
        {
            Results = results;
            UserId = userId;
            UserName = userName;
            UserLookup = userLookup;
            UserAnimeList = userAnimeList;
            MaximumRecommendationsToReturn = maximumRecommendationsToReturn;
            MaximumRecommendersToReturn = maximumRecommendersToReturn;
            DbConnectionFactory = dbConnectionFactory;
            StreamsByAnime = new Dictionary<int, ICollection<streaming_service_anime_map>>();
            AnimeWithheld = animeWithheld;
        }

        public async Task DeclareAnimeToBeDisplayedAsync(IEnumerable<int> malAnimeIds)
        {
            if (DbConnectionFactory == null) return; // Null is passed when precompiling the views
            using (IAnimeRecsDbConnection conn = DbConnectionFactory.GetConnection())
            {
                await conn.OpenAsync().ConfigureAwait(false);
                StreamsByAnime = await conn.GetStreamsAsync(malAnimeIds).ConfigureAwait(false);
            }
        }
    }
}
