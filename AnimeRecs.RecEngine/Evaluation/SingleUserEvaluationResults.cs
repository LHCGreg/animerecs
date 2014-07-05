using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.Evaluation
{
    internal class SingleUserEvaluationResults
    {
        public int TruePositives { get; set; }
        public int FalsePositives { get; set; }
        public int Unknowns { get; set; }
        public int FalseNegatives { get; set; }
        public int NumPredictions { get { return TruePositives + FalsePositives + Unknowns; } }

        public double? Precision
        {
            get
            {
                return TruePositives + FalsePositives > 0 ? ((double)TruePositives) / (TruePositives + FalsePositives) : (double?)null;
            }
        }

        public double? Recall
        {
            get
            {
                return TruePositives + FalseNegatives > 0 ? ((double)TruePositives) / (TruePositives + FalseNegatives) : (double?)null;
            }
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.
//
// AnimeRecs.RecEngine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.  If not, see <http://www.gnu.org/licenses/>.