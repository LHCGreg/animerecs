using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace AnimeRecs.RecService.Client
{
    internal class Config
    {
        public ApiType MalApiType { get; set; } = ApiType.Normal;
        public ConfigConnectionStrings ConnectionStrings { get; set; }

        internal static Config LoadFromFile(string filePath)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddXmlFile(filePath);

            IConfigurationRoot rawConfig = configBuilder.Build();
            return rawConfig.Get<Config>();
        }

        public class ConfigConnectionStrings
        {
            public string AnimeRecs { get; set; }
        }

        public enum ApiType
        {
            Normal,
            DB
        }
    }
}
