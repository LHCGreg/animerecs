using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.Web
{
    public interface IConfig
    {
        int Port { get; }
        IList<AlgorithmConfig> Algorithms { get; }
        string DefaultAlgorithm { get; }
        TimeSpan AnimeListCacheExpiration { get; }
        int? RecServicePort { get; }
        int MaximumRecommendersToReturn { get; }
        int MaximumRecommendationsToReturn { get; }
        decimal DefaultTargetPercentile { get; }
        string MalApiUserAgentString { get; }
        TimeSpan MalTimeout { get; }
        bool UseLocalDbMalApi { get; }
        string ClubMalLink { get; }
        string HtmlBeforeBodyEnd { get; }
        string PostgresConnectionString { get; }
        bool EnableDiagnosticsDashboard { get; }
        string DiagnosticsDashboardPassword { get; }
        bool ShowErrorTraces { get; }
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