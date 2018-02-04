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
