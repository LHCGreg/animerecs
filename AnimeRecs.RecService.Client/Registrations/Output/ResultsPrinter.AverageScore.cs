using System;
using System.Collections.Generic;
using System.Linq;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.ClientLib;

namespace AnimeRecs.RecService.Client.Registrations.Output
{
    internal partial class ResultsPrinter
    {
        private void PrintAverageScoreResults(MalRecResults<IEnumerable<IRecommendation>> basicResults, IDictionary<int, MalListEntry> animeList, decimal targetScore)
        {
            MalRecResults<IEnumerable<AverageScoreRecommendation>> results = basicResults.CastRecs<IEnumerable<AverageScoreRecommendation>>();
            int recNumber = 1;
            foreach (AverageScoreRecommendation rec in results.Results)
            {
                if (recNumber == 1)
                {
                    Console.WriteLine("     {0,-52} {1,-6} {2}", "Anime", "Avg", "# ratings");
                }

                Console.WriteLine("{0,3}. {1,-52} {2,-6:f2} {3}", recNumber, results.AnimeInfo[rec.ItemId].Title, rec.AverageScore, rec.NumRatings);
                recNumber++;
            }
        }
    }
}
