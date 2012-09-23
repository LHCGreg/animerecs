using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public static class RecSourceTypes
    {
        public const string AverageScore = "AverageScore";
        public const string MostPopular = "MostPopular";
        public const string AnimeRecs = "AnimeRecs";
        public const string BiasedMatrixFactorization = "BiasedMatrixFactorization";
        public const string BPRMF = "BPRMF";

        private static IDictionary<string, Func<LoadRecSourceRequest>> s_LoadRecSourceRequestFactories =
            new Dictionary<string, Func<LoadRecSourceRequest>>(StringComparer.OrdinalIgnoreCase)
            {
                { AverageScore, () => new LoadRecSourceRequest<AverageScoreRecSourceParams>() },
                { MostPopular, () => new LoadRecSourceRequest<MostPopularRecSourceParams>() },
                { AnimeRecs, () => new LoadRecSourceRequest<AnimeRecsRecSourceParams>() },
                { BiasedMatrixFactorization, () => new LoadRecSourceRequest<BiasedMatrixFactorizationRecSourceParams>() },
                { BPRMF, () => new LoadRecSourceRequest<BPRMFRecSourceParams>() }
            };
        public static IDictionary<string, Func<LoadRecSourceRequest>> LoadRecSourceRequestFactories { get { return s_LoadRecSourceRequestFactories; } }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.DTO.
//
// AnimeRecs.RecService.DTO is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.DTO is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.DTO.  If not, see <http://www.gnu.org/licenses/>.