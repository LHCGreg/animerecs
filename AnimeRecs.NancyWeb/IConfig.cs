using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.NancyWeb
{
    public interface IConfig
    {
        TimeSpan AnimeListCacheExpiration { get; }
        int? RecServicePort { get; }
        string DefaultRecSource { get; }
        int MaximumRecommendersToReturn { get; }
        int MaximumRecommendationsToReturn { get; }
        decimal DefaultTargetPercentile { get; }
        string MalApiUserAgentString { get; }
        int MalTimeoutInMs { get; }
        bool UseLocalDbMalApi { get; }
        string ClubMalLink { get; }
        string HtmlBeforeBodyEnd { get; }
        string PostgresConnectionString { get; }
        IDictionary<string, int> SpecialRecSourcePorts { get; }
        bool EnableDiagnosticsDashboard { get; }
        string DiagnosticsDashboardPassword { get; }
        bool ShowErrorTraces { get; }
    }
}
