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
