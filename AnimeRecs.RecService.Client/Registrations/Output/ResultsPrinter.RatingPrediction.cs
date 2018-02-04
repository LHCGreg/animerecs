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
        private void PrintRatingPredictionResults(MalRecResults<IEnumerable<IRecommendation>> basicResults, IDictionary<int, MalListEntry> animeList, decimal targetScore)
        {
            MalRecResults<IEnumerable<RatingPredictionRecommendation>> results = basicResults.CastRecs<IEnumerable<RatingPredictionRecommendation>>();
            int recNumber = 1;
            foreach (RatingPredictionRecommendation rec in results.Results)
            {
                if (recNumber == 1)
                {
                    Console.WriteLine("     {0,-60} {1}", "Anime", "Prediction");
                }
                Console.WriteLine("{0,3}. {1,-60} {2:F3}", recNumber, results.AnimeInfo[rec.ItemId].Title, rec.PredictedRating);
                recNumber++;
            }
        }
    }
}
