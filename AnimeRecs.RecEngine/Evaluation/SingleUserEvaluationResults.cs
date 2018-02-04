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
