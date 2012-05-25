using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.Evaluation
{
    public class EvaluationResults
    {
        public int TotalTruePositives { get; set; }
        public int TotalFalsePositives { get; set; }
        public int TotalUnknown { get; set; }
        public int TotalFalseNegatives { get; set; }
        public int TotalPredictions { get { return TotalTruePositives + TotalFalsePositives + TotalUnknown; } }

        public double? Precision
        {
            get
            {
                return TotalTruePositives + TotalFalsePositives > 0 ? ((double)TotalTruePositives) / (TotalTruePositives + TotalFalsePositives) : (double?)null;
            }
        }

        public double? Recall
        {
            get
            {
                return TotalTruePositives + TotalFalseNegatives > 0 ? ((double)TotalTruePositives) / (TotalTruePositives + TotalFalseNegatives) : (double?)null;
            }
        }

        public double TotalPrecision { get; set; }
        public int NumPrecision { get; set; }
        public double? AveragePrecision { get { return NumPrecision > 0 ? TotalPrecision / NumPrecision : (double?)null; } }

        public double TotalRecall { get; set; }
        public int NumRecall { get; set; }
        public double? AverageRecall { get { return NumRecall > 0 ? TotalRecall / NumRecall : (double?)null; } }
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