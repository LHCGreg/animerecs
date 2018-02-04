using System;
using System.Collections.Generic;
using System.Linq;
using AnimeRecs.RecService.DTO;
using Microsoft.Extensions.Configuration;

namespace AnimeRecs.RecService
{
    class Config
    {
        public class ConnectionStringsConfig
        {
            public string AnimeRecs { get; set; }
        }

        public ConnectionStringsConfig ConnectionStrings { get; set; }
        public string LoggingConfigPath { get; set; }
        public bool FinalizeAfterLoad { get; set; }
        public IList<LoadRecSourceRequest> RecSources { get; set; }

        public Config()
        {
            ConnectionStrings = new ConnectionStringsConfig();
            FinalizeAfterLoad = false;
            RecSources = new List<LoadRecSourceRequest>();
        }

        public static Config LoadFromFile(string filePath)
        {
            Config config = new Config();

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddXmlFile(filePath);

            IConfigurationRoot rawConfig = configBuilder.Build();
            config.ConnectionStrings.AnimeRecs = rawConfig.GetValue<string>("ConnectionStrings:AnimeRecs");
            config.FinalizeAfterLoad = rawConfig.GetValue<bool>("FinalizeAfterLoad");
            config.LoggingConfigPath = rawConfig.GetValue<string>("LoggingConfigPath");

            IConfigurationSection recSourcesSection = rawConfig.GetSection("RecSources");
            foreach (IConfigurationSection recSourceSection in recSourcesSection.GetChildren())
            {
                LoadRecSourceRequest recSource = ConvertRecSourceSection(recSourceSection);
                config.RecSources.Add(recSource);
            }

            return config;
        }

        private static LoadRecSourceRequest ConvertRecSourceSection(IConfigurationSection recSourceSection)
        {
            string recSourceName = recSourceSection.Key;
            string recSourceType = recSourceSection.GetValue<string>("Type");

            if (!RecSourceTypes.LoadRecSourceRequestFactories.TryGetValue(recSourceType, out Func<LoadRecSourceRequest> recSourceRequestFactory))
            {
                throw new Exception(string.Format("There is no rec source type called \"{0}\".", recSourceType));
            }

            LoadRecSourceRequest recSourceRequest = recSourceRequestFactory();
            recSourceRequest.Name = recSourceName;
            recSourceRequest.Type = recSourceType;

            RecSourceParams recSourceParams = recSourceRequest.GetParams();
            IConfigurationSection paramsSection = recSourceSection.GetSection("Params");
            paramsSection.Bind(recSourceParams);
            return recSourceRequest;
        }
    }
}
