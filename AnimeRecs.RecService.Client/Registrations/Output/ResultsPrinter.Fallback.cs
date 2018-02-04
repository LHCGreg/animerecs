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
        private void PrintFallbackResults(MalRecResults<IEnumerable<IRecommendation>> results, IDictionary<int, MalListEntry> animeList, decimal targetScore)
        {
            int recNumber = 1;
            foreach (IRecommendation rec in results.Results)
            {
                if (recNumber == 1)
                {
                    Console.WriteLine("     {0,-65}", "Anime");
                }

                Console.WriteLine("{0,3}. {1.-65}", recNumber, results.AnimeInfo[rec.ItemId].Title);
                recNumber++;
            }
        }
    }
}
