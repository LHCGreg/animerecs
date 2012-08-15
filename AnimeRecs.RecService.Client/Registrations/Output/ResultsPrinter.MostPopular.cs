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
        private void PrintMostPopularResults(MalRecResults<IEnumerable<IRecommendation>> basicResults, IDictionary<int, MalListEntry> animeList, decimal targetScore)
        {
            MalRecResults<IEnumerable<MostPopularRecommendation>> results = basicResults.CastRecs<IEnumerable<MostPopularRecommendation>>();
            int recNumber = 1;
            foreach (var recIt in results.Results.AsSmartEnumerable())
            {
                MostPopularRecommendation rec = recIt.Value;
                if (recIt.IsFirst)
                {
                    Console.WriteLine("     {0,-52} {1,4} {2}", "Anime", "Rank", "# ratings");
                }

                Console.WriteLine("{0,3}. {1,-52} {2,4} {3}", recNumber, results.AnimeInfo[rec.ItemId].Title, rec.PopularityRank, rec.NumRatings);
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