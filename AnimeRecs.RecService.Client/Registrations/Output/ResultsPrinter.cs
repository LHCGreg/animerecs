using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.ClientLib;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.RecService.Client.Registrations.Output
{
    internal partial class ResultsPrinter
    {
        private Dictionary<string, Action<MalRecResults<IEnumerable<IRecommendation>>, IDictionary<int, MalListEntry>, decimal>> RegisteredPrinters;

        public ResultsPrinter()
        {
            RegisteredPrinters = new Dictionary<string, Action<MalRecResults<IEnumerable<IRecommendation>>, IDictionary<int, MalListEntry>, decimal>>(StringComparer.OrdinalIgnoreCase)
            {
                { DTO.RecommendationTypes.AverageScore, PrintAverageScoreResults },
                { DTO.RecommendationTypes.MostPopular, PrintMostPopularResults },
                { DTO.RecommendationTypes.RatingPrediction, PrintRatingPredictionResults },
                { DTO.RecommendationTypes.AnimeRecs, PrintAnimeRecsResults }
            };
        }

        public void PrintResults(MalRecResults<IEnumerable<IRecommendation>> results, IDictionary<int, MalListEntry> animeList, decimal targetScore)
        {
            if (RegisteredPrinters.ContainsKey(results.RecommendationType))
            {
                RegisteredPrinters[results.RecommendationType](results, animeList, targetScore);
            }
            else
            {
                PrintFallbackResults(results, animeList, targetScore);
            }
        }
    }
}
