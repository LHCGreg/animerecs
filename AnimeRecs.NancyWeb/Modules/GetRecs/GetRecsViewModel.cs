using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.ClientLib;
using MalApi;

namespace AnimeRecs.NancyWeb.Modules.GetRecs
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

        public void DeclareAnimeToBeDisplayed(IEnumerable<int> malAnimeIds)
        {
            using (IAnimeRecsDbConnection conn = DbConnectionFactory.GetConnection())
            {
                StreamsByAnime = conn.GetStreams(malAnimeIds);
            }
        }
    }
}

// Copyright (C) 2014 Greg Najda
//
// This file is part of AnimeRecs.NancyWeb.
//
// AnimeRecs.NancyWeb is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.NancyWeb is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.NancyWeb.  If not, see <http://www.gnu.org/licenses/>.