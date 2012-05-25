using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.MalApi;
using AnimeRecs.RecEngine.Evaluation;
using MyMediaLite.Util;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalUserListEntries : IInputForUser, IInputForUserWithItemIds
    {
        public IDictionary<int, MalListEntry> Entries { get; private set; }
        public string MalUsername { get; private set; }

        public IDictionary<int, MalAnime> Animes { get; private set; }

        ICollection<int> IInputForUserWithItemIds.ItemIds { get { return Entries.Keys; } }

        public MalUserListEntries(IDictionary<int, MalListEntry> ratings, IDictionary<int, MalAnime> animes, string malUsername)
        {
            Entries = ratings;
            Animes = animes;
            MalUsername = malUsername;
        }

        public MalUserListEntries(MalUserLookupResults apiLookup, IDictionary<int, MalAnime> animes)
        {
            MalUsername = apiLookup.CanonicalUserName;
            Animes = animes;
            Entries = new Dictionary<int, MalListEntry>();

            foreach (MyAnimeListEntry entry in apiLookup.AnimeList)
            {
                Entries[entry.AnimeInfo.AnimeId] = new MalListEntry(entry.Score, entry.Status, entry.NumEpisodesWatched);
            }
        }

        public IBasicInputForUser AsBasicInput(int minEpisodesWatchedToCount, bool includeDropped)
        {
            Dictionary<int, float> basicRatings = new Dictionary<int,float>();
            foreach (KeyValuePair<int, MalListEntry> malListEntry in Entries)
            {
                int malAnimeId = malListEntry.Key;
                MalListEntry entry = malListEntry.Value;

                // Only use a rating if the user completed the anime or has seen at least N episodes

                if (entry.Rating != null
                    && ((entry.Status == CompletionStatus.Completed || entry.NumEpisodesWatched >= minEpisodesWatchedToCount)
                    ||  (includeDropped && entry.Status == CompletionStatus.Dropped)))
                {
                    basicRatings[malAnimeId] = (float)entry.Rating.Value;
                }
            }

            return new BasicInputForUserWithOkToRecommendPredicate(basicRatings, ItemIsOkToRecommend);
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            return (!Entries.ContainsKey(itemId) || Entries[itemId].Status == CompletionStatus.PlanToWatch)
                && Animes[itemId].Type != MalAnimeType.Special;
        }

        public bool ContainsItem(int itemId)
        {
            return Entries.ContainsKey(itemId);
        }

        public static ItemsForInputAndEvaluation<MalUserListEntries> DivideClassifiedForInputAndEvaluation(
            ClassifiedUserInput<MalUserListEntries> classifiedInput, double fractionToSetAsideForEvaluation)
        {
            IDictionary<int, MalListEntry> entriesForInput = new Dictionary<int, MalListEntry>();
            HashSet<int> likedAnimesForEvaluation = new HashSet<int>();
            HashSet<int> unlikedAnimesForEvaluation = new HashSet<int>();

            // Recommender could potentially use someone's "plan to watch" list to infer information about the user's taste...or something.
            foreach (KeyValuePair<int, MalListEntry> otherEntry in classifiedInput.Other.Entries)
            {
                entriesForInput.Add(otherEntry);
            }

            List<int> likedAnimeIds = new List<int>(classifiedInput.Liked.Entries.Keys);
            List<int> unlikedAnimeIds = new List<int>(classifiedInput.NotLiked.Entries.Keys);

            likedAnimeIds.Shuffle();
            unlikedAnimeIds.Shuffle();

            int numLikedForEvaluation = (int)(likedAnimeIds.Count * fractionToSetAsideForEvaluation);
            int numUnlikedForEvaluation = (int)(unlikedAnimeIds.Count * fractionToSetAsideForEvaluation);

            for (int i = 0; i < numLikedForEvaluation; i++)
            {
                likedAnimesForEvaluation.Add(likedAnimeIds[i]);
            }
            for (int i = numLikedForEvaluation; i < likedAnimeIds.Count; i++)
            {
                entriesForInput[likedAnimeIds[i]] = classifiedInput.Liked.Entries[likedAnimeIds[i]];
            }

            for (int i = 0; i < numUnlikedForEvaluation; i++)
            {
                unlikedAnimesForEvaluation.Add(unlikedAnimeIds[i]);
            }
            for (int i = numUnlikedForEvaluation; i < unlikedAnimeIds.Count; i++)
            {
                entriesForInput[unlikedAnimeIds[i]] = classifiedInput.NotLiked.Entries[unlikedAnimeIds[i]];
            }

            return new ItemsForInputAndEvaluation<MalUserListEntries>()
            {
                ItemsForInput = new MalUserListEntries(entriesForInput, classifiedInput.Liked.Animes, classifiedInput.Liked.MalUsername),
                LikedItemsForEvaluation = likedAnimesForEvaluation,
                UnlikedItemsForEvaluation = unlikedAnimesForEvaluation
            };
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} animes", MalUsername, Entries.Count);
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