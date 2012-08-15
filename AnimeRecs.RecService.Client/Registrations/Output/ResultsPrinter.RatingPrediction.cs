using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiscUtil.Collections.Extensions;
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
            foreach (var recIt in results.Results.AsSmartEnumerable())
            {
                RatingPredictionRecommendation rec = recIt.Value;
                if (recIt.IsFirst)
                {
                    Console.WriteLine("     {0,-60} {1}", "Anime", "Prediction");
                }
                Console.WriteLine("{0,3}. {1,-60} {2:F3}", recNumber, results.AnimeInfo[rec.ItemId].Title, rec.PredictedRating);
                recNumber++;
            }
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.Client.
//
// AnimeRecs.RecService.Client is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.Client is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.Client.  If not, see <http://www.gnu.org/licenses/>.