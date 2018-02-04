using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using Microsoft.Extensions.Options;

namespace AnimeRecs.Web
{
    public class ConfigBasedAnimeRecsDbConnectionFactory : IAnimeRecsDbConnectionFactory
    {
        private IOptionsSnapshot<Config.ConnectionStringsConfig> _config;

        public ConfigBasedAnimeRecsDbConnectionFactory(IOptionsSnapshot<Config.ConnectionStringsConfig> config)
        {
            _config = config;
        }

        public IAnimeRecsDbConnection GetConnection()
        {
            string connectionString = _config.Value.AnimeRecs;
            return new AnimeRecsDbConnection(connectionString);
        }
    }
}
