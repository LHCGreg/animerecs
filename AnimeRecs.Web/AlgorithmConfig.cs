using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.Web
{
    public class AlgorithmConfig
    {
        public string DisplayName { get; private set; }
        public string RecServiceName { get; private set; }
        public bool TargetScoreNeeded { get; private set; }
        public bool ShowDetails { get; private set; }
        public int? Port { get; private set; }

        public AlgorithmConfig(string displayName, string recServiceName, bool targetScoreNeeded, bool showDetails, int? port)
        {
            DisplayName = displayName;
            RecServiceName = recServiceName;
            TargetScoreNeeded = targetScoreNeeded;
            ShowDetails = showDetails;
            Port = port;
        }

        public static AlgorithmConfig SelectAlgorithm(ICollection<AlgorithmConfig> algorithms, string recServiceName, bool? detailsRequested, string defaultAlgorithm)
        {
            AlgorithmConfig algorithm = null;
            if (recServiceName != null)
            {
                // Match on recservice name
                List<AlgorithmConfig> algorithmsWithMatchingName = algorithms.Where(alg => alg.RecServiceName.Equals(recServiceName, StringComparison.OrdinalIgnoreCase)).ToList();

                // Then by whether to show details. Default to not showing.
                // If not found, just use the one with a matching name.

                algorithm = algorithmsWithMatchingName.Where(alg => alg.ShowDetails == (detailsRequested ?? false)).FirstOrDefault();
                if (algorithm == null && algorithmsWithMatchingName.Count > 0)
                {
                    algorithm = algorithmsWithMatchingName[0];
                }
            }

            // If we didn't find one by name or algorithm was not specified, use the default

            if (algorithm == null)
            {
                algorithm = algorithms.Where(alg => alg.DisplayName == defaultAlgorithm).FirstOrDefault();
                if (algorithm == null)
                {
                    throw new Exception(string.Format("No match for default algorithm {0}!", defaultAlgorithm));
                }

                // If detailedResults specified, look for one the same as the default but with details true or false as specified.
                // If not found, just use the default.
                if (detailsRequested != null)
                {
                    algorithm = algorithms.Where(alg => alg.RecServiceName == algorithm.RecServiceName && alg.ShowDetails == detailsRequested.Value).FirstOrDefault() ?? algorithm;
                }
            }

            return algorithm;
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