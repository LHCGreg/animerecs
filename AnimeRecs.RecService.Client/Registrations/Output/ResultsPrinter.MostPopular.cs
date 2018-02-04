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
        private void PrintMostPopularResults(MalRecResults<IEnumerable<IRecommendation>> basicResults, IDictionary<int, MalListEntry> animeList, decimal targetScore)
        {
            MalRecResults<IEnumerable<MostPopularRecommendation>> results = basicResults.CastRecs<IEnumerable<MostPopularRecommendation>>();
            int recNumber = 1;
            foreach (MostPopularRecommendation rec in results.Results)
            {
                if (recNumber == 1)
                {
                    Console.WriteLine("     {0,-52} {1,4} {2}", "Anime", "Rank", "# ratings");
                }

                Console.WriteLine("{0,3}. {1,-52} {2,4} {3}", recNumber, results.AnimeInfo[rec.ItemId].Title, rec.PopularityRank, rec.NumRatings);
                recNumber++;
            }
        }
    }
}
