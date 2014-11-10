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
        public TimeSpan AnimeListCacheExpiration { get; private set; }
        public int? RecServicePort { get; private set; }
        public string DefaultRecSource { get; private set; }
        public int MaximumRecommendersToReturn { get; private set; }
        public int MaximumRecommendationsToReturn { get; private set; }
        public decimal DefaultTargetPercentile { get; private set; }
        public string MalApiUserAgentString { get; private set; }
        public int MalTimeoutInMs { get; private set; }
        public bool UseLocalDbMalApi { get; private set; }
        public string ClubMalLink { get; private set; }
        public string HtmlBeforeBodyEnd { get; private set; }
        public string PostgresConnectionString { get; private set; }
        public IDictionary<string, int> SpecialRecSourcePorts { get; private set; }
        public bool EnableDiagnosticsDashboard { get; private set; }
        public string DiagnosticsDashboardPassword { get; private set; }
        public bool ShowErrorTraces { get; private set; }
        public bool HandleStaticContent { get; private set; }

        public Config(TimeSpan animeListCacheExpiration, int? recServicePort, string defaultRecSource, int maximumRecommendersToReturn,
            int maximumRecommendationsToReturn, decimal defaultTargetPercentile, string malApiUserAgentString, int malTimeoutInMs,
            bool useLocalDbMalApi, string clubMalLink, string htmlBeforeBodyEnd, string postgresConnectionString,
            IDictionary<string, int> specialRecSourcePorts, bool enableDiagnosticsDashboard,
            string diagnosticsDashboardPassword, bool showErrorTraces, bool handleStaticContent)
        {
            AnimeListCacheExpiration = animeListCacheExpiration;
            RecServicePort = recServicePort;

            defaultRecSource.ThrowIfNull("defaultRecSource");
            DefaultRecSource = defaultRecSource;

            MaximumRecommendersToReturn = maximumRecommendersToReturn;
            MaximumRecommendationsToReturn = maximumRecommendationsToReturn;
            DefaultTargetPercentile = defaultTargetPercentile;

            malApiUserAgentString.ThrowIfNull("malApiUserAgentString");
            MalApiUserAgentString = malApiUserAgentString;
            MalTimeoutInMs = malTimeoutInMs;
            UseLocalDbMalApi = useLocalDbMalApi;

            if (useLocalDbMalApi)
            {
                postgresConnectionString.ThrowIfNull("postgresConnectionString");
            }
            PostgresConnectionString = postgresConnectionString;

            clubMalLink.ThrowIfNull("clubMalLink");
            ClubMalLink = clubMalLink;

            HtmlBeforeBodyEnd = htmlBeforeBodyEnd ?? "";

            SpecialRecSourcePorts = specialRecSourcePorts;

            EnableDiagnosticsDashboard = enableDiagnosticsDashboard;
            DiagnosticsDashboardPassword = diagnosticsDashboardPassword;
            ShowErrorTraces = showErrorTraces;
            HandleStaticContent = handleStaticContent;
        }

        public static Config FromAppConfig()
        {
            int malCacheExpirationSeconds = int.Parse(ConfigurationManager.AppSettings["AnimeListCacheExpiration.Seconds"], CultureInfo.InvariantCulture);
            int malCacheExpirationMinutes = int.Parse(ConfigurationManager.AppSettings["AnimeListCacheExpiration.Minutes"], CultureInfo.InvariantCulture);
            TimeSpan malCacheExpiration = new TimeSpan(hours: 0, minutes: malCacheExpirationMinutes, seconds: malCacheExpirationSeconds);

            int? recServicePort = null;
            if (ConfigurationManager.AppSettings["RecService.Port"] != null)
            {
                recServicePort = int.Parse(ConfigurationManager.AppSettings["RecService.Port"], CultureInfo.InvariantCulture);
            }

            string defaultRecSource = ConfigurationManager.AppSettings["RecService.DefaultRecSource"];

            int maxRecommendersToReturn = int.Parse(ConfigurationManager.AppSettings["MaximumRecommendersToReturn"], CultureInfo.InvariantCulture);
            int maxRecommendationsToReturn = int.Parse(ConfigurationManager.AppSettings["MaximumRecommendationsToReturn"], CultureInfo.InvariantCulture);
            decimal defaultTargetPercentile = decimal.Parse(ConfigurationManager.AppSettings["DefaultTargetPercentile"], CultureInfo.InvariantCulture) / 100;
            string malApiUserAgentString = ConfigurationManager.AppSettings["MalApi.UserAgentString"];
            int malTimeoutInMs = int.Parse(ConfigurationManager.AppSettings["MalApi.TimeoutInMs"]);
            bool useLocalDbMalApi = false;
            if (ConfigurationManager.AppSettings["MalApi.API"] != null && ConfigurationManager.AppSettings["MalApi.API"].Equals("DB"))
            {
                useLocalDbMalApi = true;
            }
            string clubMalLink = ConfigurationManager.AppSettings["ClubMalLink"];

            string postgresConnectionString = null;
            if (ConfigurationManager.ConnectionStrings["Postgres"] != null)
            {
                postgresConnectionString = ConfigurationManager.ConnectionStrings["Postgres"].ConnectionString;
            }

            string htmlBeforeBodyEnd = "";
            if (ConfigurationManager.AppSettings["HtmlBeforeBodyEnd"] != null)
            {
                htmlBeforeBodyEnd = ConfigurationManager.AppSettings["HtmlBeforeBodyEnd"];
            }

            Dictionary<string, int> specialRecSourcePorts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            Regex specialRecSourcePortConfigKeyRegex = new Regex(@"RecService\.(?<RecSource>.+)\.Port");
            foreach (string configKey in ConfigurationManager.AppSettings.AllKeys)
            {
                Match m = specialRecSourcePortConfigKeyRegex.Match(configKey);
                if (m.Success)
                {
                    string recSourceName = m.Groups["RecSource"].Value;
                    specialRecSourcePorts[recSourceName] = int.Parse(ConfigurationManager.AppSettings[configKey]);
                }
            }

            bool enableDiagnosticsDashboard = false;
            string diagnosticsDashboardPassword = "";
            if (ConfigurationManager.AppSettings["Diagnostics.EnableDiagnosticsDashboard"] != null)
            {
                enableDiagnosticsDashboard = bool.Parse(ConfigurationManager.AppSettings["Diagnostics.EnableDiagnosticsDashboard"]);
                diagnosticsDashboardPassword = ConfigurationManager.AppSettings["Diagnostics.DiagnosticsDashboardPassword"];
            }

            bool showErrorTraces = false;
            if (ConfigurationManager.AppSettings["Diagnostics.ShowErrorTraces"] != null)
            {
                showErrorTraces = bool.Parse(ConfigurationManager.AppSettings["Diagnostics.ShowErrorTraces"]);
            }

            bool handleStaticContent = true;
            if (ConfigurationManager.AppSettings["Hosting.HandleStaticContent"] != null)
            {
                handleStaticContent = bool.Parse(ConfigurationManager.AppSettings["Hosting.HandleStaticContent"]);
            }

            return new Config
            (
                animeListCacheExpiration: malCacheExpiration,
                recServicePort: recServicePort,
                defaultRecSource: defaultRecSource,
                maximumRecommendersToReturn: maxRecommendersToReturn,
                maximumRecommendationsToReturn: maxRecommendationsToReturn,
                defaultTargetPercentile: defaultTargetPercentile,
                malApiUserAgentString: malApiUserAgentString,
                malTimeoutInMs: malTimeoutInMs,
                useLocalDbMalApi: useLocalDbMalApi,
                clubMalLink: clubMalLink,
                htmlBeforeBodyEnd: htmlBeforeBodyEnd,
                postgresConnectionString: postgresConnectionString,
                specialRecSourcePorts: specialRecSourcePorts,
                enableDiagnosticsDashboard: enableDiagnosticsDashboard,
                diagnosticsDashboardPassword: diagnosticsDashboardPassword,
                showErrorTraces: showErrorTraces,
                handleStaticContent: handleStaticContent
            );
        }
    }
}

// Copyright (C) 2014 Greg Najda
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