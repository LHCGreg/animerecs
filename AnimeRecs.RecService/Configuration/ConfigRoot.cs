using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;
using Newtonsoft.Json;
using System.IO;

namespace AnimeRecs.RecService.Configuration
{
    class ConfigRoot
    {
        public IList<LoadRecSourceRequest> RecSources { get; set; }

        public ConfigRoot()
        {
            RecSources = new List<LoadRecSourceRequest>();
        }

        public ConfigRoot(IList<LoadRecSourceRequest> recSources)
        {
            RecSources = recSources;
        }

        public static ConfigRoot LoadFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            ConfigRoot config = JsonConvert.DeserializeObject<ConfigRoot>(json);
            foreach (LoadRecSourceRequest recSource in config.RecSources)
            {

            }
        }
    }
}
