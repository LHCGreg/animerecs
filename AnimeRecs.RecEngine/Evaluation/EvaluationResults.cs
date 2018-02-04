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
