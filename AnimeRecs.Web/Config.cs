using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MiscUtil.Extensions;

namespace AnimeRecs.Web
{
    public class Config : IConfig
    {
        public int Port { get; private set; }
        public IList<AlgorithmConfig> Algorithms { get; private set; }
        public string DefaultAlgorithm { get; private set; }
        public TimeSpan AnimeListCacheExpiration { get; private set; }
        public int? RecServicePort { get; private set; }
        public int MaximumRecommendersToReturn { get; private set; }
        public int MaximumRecommendationsToReturn { get; private set; }
        public decimal DefaultTargetPercentile { get; private set; }
        public string MalApiUserAgentString { get; private set; }
        public TimeSpan MalTimeout { get; private set; }
        public bool UseLocalDbMalApi { get; private set; }
        public string ClubMalLink { get; private set; }
        public string HtmlBeforeBodyEnd { get; private set; }
        public string PostgresConnectionString { get; private set; }
        public bool EnableDiagnosticsDashboard { get; private set; }
        public string DiagnosticsDashboardPassword { get; private set; }
        public bool ShowErrorTraces { get; private set; }
        public bool HandleStaticContent { get; private set; }

        public Config(int port, IList<AlgorithmConfig> algorithms, string defaultAlgorithm, TimeSpan animeListCacheExpiration, int? recServicePort, int maximumRecommendersToReturn,
            int maximumRecommendationsToReturn, decimal defaultTargetPercentile, string malApiUserAgentString, TimeSpan malTimeout,
            bool useLocalDbMalApi, string clubMalLink, string htmlBeforeBodyEnd, string postgresConnectionString,
            bool enableDiagnosticsDashboard,
            string diagnosticsDashboardPassword, bool showErrorTraces, bool handleStaticContent)
        {
            Port = port;
            Algorithms = algorithms;
            DefaultAlgorithm = defaultAlgorithm;
            AnimeListCacheExpiration = animeListCacheExpiration;
            RecServicePort = recServicePort;

            MaximumRecommendersToReturn = maximumRecommendersToReturn;
            MaximumRecommendationsToReturn = maximumRecommendationsToReturn;
            DefaultTargetPercentile = defaultTargetPercentile;

            malApiUserAgentString.ThrowIfNull("malApiUserAgentString");
            MalApiUserAgentString = malApiUserAgentString;
            MalTimeout = malTimeout;
            UseLocalDbMalApi = useLocalDbMalApi;

            if (useLocalDbMalApi)
            {
                postgresConnectionString.ThrowIfNull("postgresConnectionString");
            }
            PostgresConnectionString = postgresConnectionString;

            clubMalLink.ThrowIfNull("clubMalLink");
            ClubMalLink = clubMalLink;

            HtmlBeforeBodyEnd = htmlBeforeBodyEnd ?? "";

            EnableDiagnosticsDashboard = enableDiagnosticsDashboard;
            DiagnosticsDashboardPassword = diagnosticsDashboardPassword;
            ShowErrorTraces = showErrorTraces;
            HandleStaticContent = handleStaticContent;
        }

        public static Config FromAppConfig()
        {
            Logging.Log.Debug("Loading config");
            AnimeRecsConfigurationSection appConfig = AnimeRecsConfigurationSection.Settings;

            List<AlgorithmConfig> algorithms = new List<AlgorithmConfig>();
            foreach (AlgorithmElement algorithm in appConfig.Algorithms)
            {
                algorithms.Add(new AlgorithmConfig(algorithm.DisplayName, algorithm.RecServiceName, algorithm.TargetScoreNeeded, algorithm.Details, algorithm.Port));
            }

            string postgresConnectionString = null;
            if (ConfigurationManager.ConnectionStrings["Postgres"] != null)
            {
                postgresConnectionString = ConfigurationManager.ConnectionStrings["Postgres"].ConnectionString;
            }

            Config config = new Config
            (
                port: appConfig.Hosting.Port,
                algorithms: algorithms,
                defaultAlgorithm: appConfig.Algorithms.Default,
                animeListCacheExpiration: appConfig.MalAPI.CacheExpiration,
                recServicePort: appConfig.RecService.Port,
                maximumRecommendersToReturn: appConfig.Recommendations.MaxRecommendersToReturn,
                maximumRecommendationsToReturn: appConfig.Recommendations.MaxRecommendationsToReturn,
                defaultTargetPercentile: appConfig.Recommendations.DefaultTargetPercentile,
                malApiUserAgentString: appConfig.MalAPI.UserAgentString,
                malTimeout: appConfig.MalAPI.Timeout,
                useLocalDbMalApi: appConfig.MalAPI.Type == MalAPIType.DB,
                clubMalLink: appConfig.Html.ClubMalLink,
                htmlBeforeBodyEnd: appConfig.Html.HtmlBeforeBodyEnd,
                postgresConnectionString: postgresConnectionString,
                enableDiagnosticsDashboard: appConfig.Diagnostics.EnableDashboard,
                diagnosticsDashboardPassword: appConfig.Diagnostics.DashboardPassword,
                showErrorTraces: appConfig.Diagnostics.ShowErrorTraces,
                handleStaticContent: appConfig.Hosting.HandleStaticContent
            );

            Logging.Log.Debug("Config loaded.");

            return config;
        }
    }
}

// Copyright (C) 2015 Greg Najda
//
// This file is part of AnimeRecs.Web.
//
// AnimeRecs.Web is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.Web is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.Web.  If not, see <http://www.gnu.org/licenses/>.