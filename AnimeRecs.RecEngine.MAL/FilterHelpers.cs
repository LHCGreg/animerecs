using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    internal static class FilterHelpers
    {
        public static IBasicTrainingData<IBasicInputForUser> RemoveItemsWithFewUsers(IBasicTrainingData<IBasicInputForUser> trainingData, int minimumUsers)
        {
            Dictionary<int, int> itemRatingCountByItemId = new Dictionary<int, int>();
            
            foreach (int userId in trainingData.Users.Keys)
            {
                foreach (int animeId in trainingData.Users[userId].Ratings.Keys)
                {
                    if (!itemRatingCountByItemId.ContainsKey(animeId))
                    {
                        itemRatingCountByItemId[animeId] = 0;
                    }
                    itemRatingCountByItemId[animeId]++;
                }
            }

            foreach (int userId in trainingData.Users.Keys)
            {
                List<int> animeIdsToRemove = new List<int>();

                foreach (int animeId in trainingData.Users[userId].Ratings.Keys)
                {
                    if (itemRatingCountByItemId[animeId] < minimumUsers)
                    {
                        animeIdsToRemove.Add(animeId);
                    }
                }

                foreach (int animeIdToRemove in animeIdsToRemove)
                {
                    trainingData.Users[userId].Ratings.Remove(animeIdToRemove);
                }
            }

            return trainingData;
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.MAL.
//
// AnimeRecs.RecEngine.MAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine.MAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.MAL.  If not, see <http://www.gnu.org/licenses/>.