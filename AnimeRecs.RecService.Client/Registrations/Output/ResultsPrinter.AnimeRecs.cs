using System;
using System.Collections.Generic;
using System.Linq;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.ClientLib;
using MalApi;
using System.Globalization;

namespace AnimeRecs.RecService.Client.Registrations.Output
{
    internal partial class ResultsPrinter
    {
        private void PrintAnimeRecsResults(MalRecResults<IEnumerable<IRecommendation>> basicResults, IDictionary<int, MalListEntry> animeList, decimal targetScore)
        {
            MalRecResults<MalAnimeRecsResults> results = basicResults.CastRecs<MalAnimeRecsResults>();
            
            int numRecommendersPrinted = 0;

            foreach (MalAnimeRecsRecommenderUser recommender in results.Results.Recommenders)
            {
                if (numRecommendersPrinted > 10)
                {
                    break;
                }

                string recsLikedString;

                if (recommender.NumRecsWithJudgment > 0)
                {
                    recsLikedString = string.Format("{0:P2}", (double)recommender.RecsLiked.Count / recommender.NumRecsWithJudgment);
                }
                else
                {
                    recsLikedString = string.Format("{0:P2}", 0);
                }

                Console.WriteLine("{0}'s recommendations ({1}/{2} {3} recs liked, {4:P2} - {5:P2} estimated compatibility",
                    recommender.Username, recommender.RecsLiked.Count, recommender.NumRecsWithJudgment, recsLikedString,
                    recommender.CompatibilityLowEndpoint ?? 0, recommender.CompatibilityHighEndpoint ?? 0);

                Console.WriteLine("{0,-52} {1,-5} {2,-4} {3,-5} {4,-4} {5}", "Anime", "State", "Like", "Their", "Your", "Avg");

                foreach (MalAnimeRecsRecommenderRecommendation recommendation in recommender.AllRecommendations.OrderBy(
                    rec => !recommender.RecsLiked.Contains(rec) && !recommender.RecsNotLiked.Contains(rec) ? 0 :
                        recommender.RecsLiked.Contains(rec) ? 1 :
                        2
                    )
                    .ThenByDescending(rec => rec.RecommenderScore)
                    .ThenByDescending(rec => rec.AverageScore))
                {
                    string status;
                    decimal? yourRating = null;
                    if (!animeList.ContainsKey(recommendation.MalAnimeId))
                    {
                        status = "-"; // not watched, not in list
                    }
                    else
                    {
                        if (animeList[recommendation.MalAnimeId].Status == CompletionStatus.Completed)
                            status = "comp";
                        else if (animeList[recommendation.MalAnimeId].Status == CompletionStatus.Dropped)
                            status = "drop";
                        else if (animeList[recommendation.MalAnimeId].Status == CompletionStatus.OnHold)
                            status = "hold";
                        else if (animeList[recommendation.MalAnimeId].Status == CompletionStatus.PlanToWatch)
                            status = "plan";
                        else if (animeList[recommendation.MalAnimeId].Status == CompletionStatus.Watching)
                            status = "watch";
                        else
                            status = "?";

                        yourRating = animeList[recommendation.MalAnimeId].Rating;
                    }

                    string yourRatingString;
                    if (yourRating != null)
                    {
                        yourRatingString = yourRating.Value.ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        yourRatingString = "-";
                    }

                    string likedString;
                    if (recommender.RecsLiked.Contains(recommendation))
                        likedString = "+";
                    else if (recommender.RecsNotLiked.Contains(recommendation))
                        likedString = "-";
                    else
                        likedString = "?";

                    Console.WriteLine("{0,-52} {1,-5} {2,-4} {3,-5:F0} {4,-4:F0} {5:F2}",
                        results.AnimeInfo[recommendation.MalAnimeId].Title, status, likedString, recommendation.RecommenderScore,
                        yourRatingString, recommendation.AverageScore);
                }

                Console.WriteLine();
                Console.WriteLine();

                numRecommendersPrinted++;
            }
        }
    }
}

// Copyright (C) 2017 Greg Najda
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