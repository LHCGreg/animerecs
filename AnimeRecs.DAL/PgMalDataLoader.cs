using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.MalApi;

namespace AnimeRecs.DAL
{
    public class PgMalDataLoader : IMalTrainingDataLoader, IDisposable
    {
        private PetaPoco.Database m_db;
        private bool m_disposeDb;

        public PgMalDataLoader(PetaPoco.Database db, bool disposeDb = false)
        {
            m_db = db;
            m_disposeDb = disposeDb;
        }

        public PgMalDataLoader(string pgConnectionString)
        {
            m_db = new PetaPoco.Database(pgConnectionString, Npgsql.NpgsqlFactory.Instance);
            m_disposeDb = true;
        }

        public void Dispose()
        {
            if (m_db != null && m_disposeDb)
            {
                m_db.Dispose();
            }
        }

        public MalTrainingData LoadMalTrainingData()
        {
            //public int mal_anime_id { get; set; }
            //public int mal_user_id { get; set; }
            //public decimal? rating { get; set; }
            //public int mal_list_entry_status_id { get; set; }
            //public int num_episodes_watched { get; set; }
            //public int mal_anime_type_id { get; set; }
            //public string title { get; set; }
            //public string mal_name { get; set; }

            string sql = @"
SELECT rating.mal_anime_id, rating.mal_user_id, rating.rating, rating.mal_list_entry_status_id, rating.num_episodes_watched,
mal_anime.mal_anime_type_id, mal_anime.title, mal_user.mal_name
FROM mal_list_entry AS rating
JOIN mal_anime ON rating.mal_anime_id = mal_anime.mal_anime_id
JOIN mal_user ON rating.mal_user_id = mal_user.mal_user_id
";

            Dictionary<int, MalAnime> animes = new Dictionary<int, MalAnime>();

            Dictionary<int, IDictionary<int, MalListEntry>> users = new Dictionary<int, IDictionary<int, MalListEntry>>();
            Dictionary<int, string> usernames = new Dictionary<int, string>();

            foreach (MalListEntryJoined listEntry in m_db.Query<MalListEntryJoined>(sql))
            {
                if (!animes.ContainsKey(listEntry.mal_anime_id))
                {
                    animes[listEntry.mal_anime_id] = new MalAnime(listEntry.mal_anime_id, (MalAnimeType)listEntry.mal_anime_type_id, listEntry.title);
                }

                if (!users.ContainsKey(listEntry.mal_user_id))
                {
                    users[listEntry.mal_user_id] = new Dictionary<int, MalListEntry>();
                    usernames[listEntry.mal_user_id] = listEntry.mal_name;
                }

                users[listEntry.mal_user_id][listEntry.mal_anime_id] = new MalListEntry(listEntry.rating, (CompletionStatus)listEntry.mal_list_entry_status_id,
                    listEntry.num_episodes_watched);
            }

            Dictionary<int, MalUserListEntries> ratings = new Dictionary<int, MalUserListEntries>();
            foreach (int userId in users.Keys)
            {
                ratings[userId] = new MalUserListEntries(ratings: users[userId], animes: animes, malUsername: usernames[userId]);
            }

            return new MalTrainingData(ratings, animes);
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