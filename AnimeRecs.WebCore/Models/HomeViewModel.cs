using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeRecs.WebCore.Models
{
    public class HomeViewModel
    {
        public string Algorithm { get; set; }
        public bool AlgorithmAvailable { get; set; }
        public bool TargetScoreNeeded { get; set; }
        public bool DisplayDetailedResults { get; set; }
        public bool DebugModeOn { get; set; }

        public HomeViewModel(string algorithm, bool algorithmAvailable, bool targetScoreNeeded, bool displayDetailedResults,
            bool debugModeOn)
        {
            Algorithm = algorithm;
            AlgorithmAvailable = algorithmAvailable;
            TargetScoreNeeded = targetScoreNeeded;
            DisplayDetailedResults = displayDetailedResults;
            DebugModeOn = debugModeOn;
        }
    }
}
