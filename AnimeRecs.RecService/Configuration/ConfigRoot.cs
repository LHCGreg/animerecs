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
        public bool FinalizeAfterLoad { get; set; }
        public IList<LoadRecSourceRequest> RecSources { get; set; }

        public ConfigRoot()
        {
            FinalizeAfterLoad = false;
            RecSources = new List<LoadRecSourceRequest>();
        }

        public ConfigRoot(IList<LoadRecSourceRequest> recSources, bool finalizeAfterLoad)
        {
            FinalizeAfterLoad = finalizeAfterLoad;
            RecSources = recSources;
        }

        public static ConfigRoot LoadFromFile(string filePath)
        {
            using(StreamReader fileReader = new StreamReader(filePath))
            using(JsonTextReader jsonReader = new JsonTextReader(fileReader))
            {
                ConfigRoot config = new JsonSerializer().Deserialize<ConfigRoot>(jsonReader);
                return config;
            }
        }
    }
}
