using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace AnimeRecs.Web
{
    public class AnimeRecsConfigurationSection : ConfigurationSection
    {
        public static AnimeRecsConfigurationSection Settings { get { return (AnimeRecsConfigurationSection)ConfigurationManager.GetSection("animerecs"); } }

        [ConfigurationProperty("algorithms", IsRequired = true)]
        public AlgorithmCollectionElement Algorithms { get { return (AlgorithmCollectionElement)base["algorithms"]; } }

        [ConfigurationProperty("diagnostics", IsRequired = true)]
        public DiagnosticsElement Diagnostics { get { return (DiagnosticsElement)base["diagnostics"]; } }

        [ConfigurationProperty("hosting", IsRequired = true)]
        public HostingElement Hosting { get { return (HostingElement)base["hosting"]; } }

        [ConfigurationProperty("malApi", IsRequired = true)]
        public MalApiElement MalAPI { get { return (MalApiElement)base["malApi"]; } }

        [ConfigurationProperty("recommendations", IsRequired = true)]
        public RecommendationsElement Recommendations { get { return (RecommendationsElement)base["recommendations"]; } }

        [ConfigurationProperty("recService", IsRequired = true)]
        public RecServiceElement RecService { get { return (RecServiceElement)base["recService"]; } }

        [ConfigurationProperty("html", IsRequired = true)]
        public HtmlElement Html { get { return (HtmlElement)base["html"]; } }
    }

    [ConfigurationCollection(typeof(AlgorithmElement), AddItemName = "algorithm", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class AlgorithmCollectionElement : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AlgorithmElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AlgorithmElement)element).DisplayName;
        }

        [ConfigurationProperty("default", IsRequired = true)]
        public string Default { get { return (string)base["default"]; } }
    }

    public class AlgorithmElement : ConfigurationElement
    {
        [ConfigurationProperty("displayName", IsRequired = true)]
        public string DisplayName { get { return (string)base["displayName"]; } }

        [ConfigurationProperty("recServiceName", IsRequired = true)]
        public string RecServiceName { get { return (string)base["recServiceName"]; } }

        [ConfigurationProperty("details", IsRequired = false, DefaultValue = false)]
        public bool Details { get { return (bool)base["details"]; } }

        [ConfigurationProperty("targetScoreNeeded", IsRequired = false, DefaultValue = false)]
        public bool TargetScoreNeeded { get { return (bool)base["targetScoreNeeded"]; } }

        [ConfigurationProperty("port", IsRequired = false, DefaultValue = null)]
        public int? Port { get { return base["port"] != null ? (int)base["port"] : (int?)null; } }
    }

    public class DiagnosticsElement : ConfigurationElement
    {
        [ConfigurationProperty("enableDashboard", IsRequired = true)]
        public bool EnableDashboard { get { return (bool)base["enableDashboard"]; } }

        [ConfigurationProperty("dashboardPassword", IsRequired = false, DefaultValue = "diag")]
        public string DashboardPassword { get { return (string)base["dashboardPassword"]; } }

        [ConfigurationProperty("showErrorTraces", IsRequired = true)]
        public bool ShowErrorTraces { get { return (bool)base["showErrorTraces"]; } }
    }

    public class HostingElement : ConfigurationElement
    {
        [ConfigurationProperty("handleStaticContent", IsRequired = true)]
        public bool HandleStaticContent { get { return (bool)base["handleStaticContent"]; } }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port { get { return (int)base["port"]; } }
    }

    public class MalApiElement : ConfigurationElement
    {
        [ConfigurationProperty("timeout", IsRequired = true)]
        public TimeSpan Timeout { get { return (TimeSpan)base["timeout"]; } }

        [ConfigurationProperty("cacheExpiration", IsRequired = true)]
        public TimeSpan CacheExpiration { get { return (TimeSpan)base["cacheExpiration"]; } }

        [ConfigurationProperty("type", IsRequired = true, DefaultValue = "normal")]
        [CallbackValidator(Type = typeof(MalApiElement), CallbackMethodName = "ValidateType")]
        public string TypeString { get { return (string)base["type"]; } }

        [ConfigurationProperty("userAgentString", IsRequired = true)]
        public string UserAgentString { get { return (string)base["userAgentString"]; } }

        public MalAPIType Type
        {
            get
            {
                if (TypeString.Equals("normal", StringComparison.OrdinalIgnoreCase))
                {
                    return MalAPIType.Normal;
                }
                else if (TypeString.Equals("DB", StringComparison.OrdinalIgnoreCase))
                {
                    return MalAPIType.DB;
                }
                else
                {
                    throw new Exception(string.Format("type must be \"normal\" or \"DB\". Was {0}", TypeString));
                }
            }
        }

        public static void ValidateType(object typeString)
        {
            string type = (string)typeString;
            if (!type.Equals("normal", StringComparison.OrdinalIgnoreCase) && !type.Equals("DB", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(string.Format("type must be \"normal\" or \"DB\". Was {0}", type));
            }
        }
    }

    public class RecommendationsElement : ConfigurationElement
    {
        [ConfigurationProperty("maxRecommendersToReturn", IsRequired = true)]
        public int MaxRecommendersToReturn { get { return (int)base["maxRecommendersToReturn"]; } }

        [ConfigurationProperty("maxRecommendationsToReturn", IsRequired = true)]
        public int MaxRecommendationsToReturn { get { return (int)base["maxRecommendationsToReturn"]; } }

        [ConfigurationProperty("defaultTargetPercentile", IsRequired = true)]
        public decimal DefaultTargetPercentile { get { return (decimal)base["defaultTargetPercentile"]; } }
    }

    public class RecServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("port", IsRequired = false, DefaultValue = null)]
        public int? Port { get { return base["port"] != null ? (int?)base["port"] : (int?)null; } }
    }

    public class HtmlElement : ConfigurationElement
    {
        [ConfigurationProperty("clubMalLink", IsRequired = true)]
        public string ClubMalLink { get { return (string)base["clubMalLink"]; } }

        [ConfigurationProperty("htmlBeforeBodyEnd", IsRequired = false, DefaultValue = "")]
        public string HtmlBeforeBodyEnd { get { return (string)base["htmlBeforeBodyEnd"]; } }
    }

    public enum MalAPIType
    {
        Normal,
        DB
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