using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AnimeRecs.Web
{
    public class Config
    {
        public class ConnectionStringsConfig
        {
            public string AnimeRecs { get; set; }
        }

        public class HostingConfig
        {
            public int Port { get; set; }
            public string UnixSocketPath { get; set; }
            public bool HandleStaticContent { get; set; }

            public HostingConfig()
            {
                Port = 5000;
                HandleStaticContent = true;
            }
        }

        public enum MalApiType
        {
            Normal,
            DB,
        }

        public class MalApiConfig
        {
            public MalApiType Type { get; set; }
            public string UserAgentString { get; set; }
            public int TimeoutMilliseconds { get; set; }
            public int AnimeListCacheExpirationSeconds { get; set; }

            public MalApiConfig()
            {
                Type = MalApiType.Normal;
                TimeoutMilliseconds = 5000;
                AnimeListCacheExpirationSeconds = 30;
            }
        }

        public class RecommendationsConfig
        {
            public class NonDefaultRecServiceConfig
            {
                public int Port { get; set; }
            }

            public int RecServicePort { get; set; }
            public Dictionary<string, NonDefaultRecServiceConfig> NonDefaultRecServices { get; set; }
            public string DefaultRecSource { get; set; }
            public int MaximumRecommendersToReturn { get; set; }
            public int MaximumRecommendationsToReturn { get; set; }
            public decimal DefaultTargetPercentile { get; set; }
            public int TimeoutMilliseconds { get; set; }

            public RecommendationsConfig()
            {
                RecServicePort = 5541;
                NonDefaultRecServices = new Dictionary<string, NonDefaultRecServiceConfig>();
                DefaultRecSource = "default";
                MaximumRecommendersToReturn = 6;
                MaximumRecommendationsToReturn = 60;
                DefaultTargetPercentile = 35;
                TimeoutMilliseconds = 10000;
            }
        }

        public class HtmlConfig
        {
            public string ClubMalLink { get; set; }
            public string HtmlBeforeBodyEnd { get; set; }

            public HtmlConfig()
            {
                HtmlBeforeBodyEnd = "";
            }
        }

        public string LoggingConfigPath { get; set; }
        public ConnectionStringsConfig ConnectionStrings { get; set; }
        public HostingConfig Hosting { get; set; }
        public MalApiConfig MalApi { get; set; }
        public RecommendationsConfig Recommendations { get; set; }
        public HtmlConfig Html { get; set; }

        public Config()
        {
            ConnectionStrings = new ConnectionStringsConfig();
            Hosting = new HostingConfig();
            MalApi = new MalApiConfig();
            Recommendations = new RecommendationsConfig();
            Html = new HtmlConfig();
        }

        public static Config LoadFromFile(string filePath)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddXmlFile(filePath);
            IConfigurationRoot rawConfig = configBuilder.Build();
            Config config = rawConfig.Get<Config>();
            return config;
        }
    }
}
