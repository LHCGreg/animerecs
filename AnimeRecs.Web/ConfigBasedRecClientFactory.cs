using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeRecs.RecService.ClientLib;
using Microsoft.Extensions.Options;

namespace AnimeRecs.Web
{
    public class ConfigBasedRecClientFactory : IAnimeRecsClientFactory
    {
        IOptionsSnapshot<Config.RecommendationsConfig> _config;

        public ConfigBasedRecClientFactory(IOptionsSnapshot<Config.RecommendationsConfig> config)
        {
            _config = config;
        }

        public AnimeRecsClient GetClient(string recSourceName)
        {
            if (recSourceName != null && _config.Value.NonDefaultRecServices.ContainsKey(recSourceName))
            {
                int port = _config.Value.NonDefaultRecServices[recSourceName].Port;
                return new AnimeRecsClient(port);
            }
            else
            {
                return new AnimeRecsClient(_config.Value.RecServicePort);
            }
        }
    }
}
