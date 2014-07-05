using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MalApi;
using AnimeRecs.RecEngine.Evaluation;
using MyMediaLite;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalUserListEntries : IInputForUser, IInputForUserWithItemIds
    {
        public IDictionary<int, MalListEntry> Entries { get; private set; }

        /// <summary>
        /// Can be null if not known.
        /// </summary>
        public string MalUsername { get; private set; }

        public IDictionary<int, MalAnime> AnimesEligibleForRecommendation { get; private set; }

        ICollection<int> IInputForUserWithItemIds.ItemIds { get { return Entries.Keys; } }

        public AnimeOkToRecommendPredicate OkToRecommendPredicate { get; private set; }

        public MalUserListEntries(IDictionary<int, MalListEntry> ratings, IDictionary<int, MalAnime> animes, string malUsername,
            IDictionary<int, IList<int>> prerequisites)
            : this(ratings, animes, malUsername, MakeDefaultOkToRecommendPredicate(prerequisites))
        {
            ;
        }

        private static AnimeOkToRecommendPredicate MakeDefaultOkToRecommendPredicate(IDictionary<int, IList<int>> prereqs)
        {
            return (MalUserListEntries userAnimeList, int animeId) =>
                // Anime is not on the user's anime list, or it is but "planned to watch"
                (!userAnimeList.Entries.ContainsKey(animeId) || userAnimeList.Entries[animeId].Status == CompletionStatus.PlanToWatch)

                // And it's not a special
                && userAnimeList.AnimesEligibleForRecommendation[animeId].Type != MalAnimeType.Special

                // And the user has seen all of its prerequisites
                && AnimeListContainsPrerequisitesFor(userAnimeList, prereqs, animeId);
        }

        private static bool AnimeListContainsPrerequisitesFor(MalUserListEntries userAnimeList, IDictionary<int, IList<int>> prereqs,
            int animeId)
        {
            IList<int> animePrereqs;
            if (!prereqs.TryGetValue(animeId, out animePrereqs))
            {
                // No prereqs
                return true;
            }
            foreach (int prereqId in animePrereqs)
            {
                // For each prereq, user must have it on their list as completed
                if (!userAnimeList.Entries.ContainsKey(prereqId) || userAnimeList.Entries[prereqId].Status != CompletionStatus.Completed)
                {
                    return false;
                }
            }
            return true;
        }

        public MalUserListEntries(IDictionary<int, MalListEntry> ratings, IDictionary<int, MalAnime> animes, string malUsername,
            AnimeOkToRecommendPredicate okToRecommendPredicate)
        {
            Entries = ratings;
            AnimesEligibleForRecommendation = animes;
            MalUsername = malUsername;
            OkToRecommendPredicate = okToRecommendPredicate;
        }

        public IBasicInputForUser AsBasicInput(int minEpisodesWatchedToCount, bool includeDropped)
        {
            return AsBasicInput(minEpisodesWatchedToCount, includeDropped, null);
        }

        public IBasicInputForUser AsBasicInput(int minEpisodesWatchedToCount, bool includeDropped, Predicate<int> additionalOkToRecommendPredicate)
        {
            Dictionary<int, float> basicRatings = new Dictionary<int, float>();
            foreach (KeyValuePair<int, MalListEntry> malListEntry in Entries)
            {
                int malAnimeId = malListEntry.Key;
                MalListEntry entry = malListEntry.Value;

                // Only use a rating if the user completed the anime or has seen at least N episodes

                if (entry.Rating != null
                    && ((entry.Status == CompletionStatus.Completed || entry.NumEpisodesWatched >= minEpisodesWatchedToCount)
                    || (includeDropped && entry.Status == CompletionStatus.Dropped)))
                {
                    basicRatings[malAnimeId] = (float)entry.Rating.Value;
                }
            }

            if (additionalOkToRecommendPredicate == null)
            {
                return new BasicInputForUserWithOkToRecommendPredicate(basicRatings, ItemIsOkToRecommend);
            }
            else
            {
                return new BasicInputForUserWithOkToRecommendPredicate(basicRatings, (itemId) =>
                    ItemIsOkToRecommend(itemId) && additionalOkToRecommendPredicate(itemId));
            }
        }

        public IPositiveFeedbackForUser AsPositiveFeedback(IUserInputClassifier<MalUserListEntries> classifier)
        {
            return AsPositiveFeedback(classifier, null);
        }

        public IPositiveFeedbackForUser AsPositiveFeedback(IUserInputClassifier<MalUserListEntries> classifier, Predicate<int> additionalOkToRecommendPredicate)
        {
            ClassifiedUserInput<MalUserListEntries> classified = classifier.Classify(this);
            HashSet<int> basicFeedback = new HashSet<int>(classified.Liked.Entries.Select(itemIdEntryPair => itemIdEntryPair.Key));

            if (additionalOkToRecommendPredicate == null)
            {
                return new BasicPositiveFeedbackForUserWithOkToRecommendPredicate(basicFeedback, ItemIsOkToRecommend);
            }
            else
            {
                return new BasicPositiveFeedbackForUserWithOkToRecommendPredicate(basicFeedback, (itemId) =>
                    ItemIsOkToRecommend(itemId) && additionalOkToRecommendPredicate(itemId));
            }
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            if (OkToRecommendPredicate != null)
            {
                return OkToRecommendPredicate(this, itemId);
            }
            else
            {
                return true;
            }
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
                ItemsForInput = new MalUserListEntries(entriesForInput, classifiedInput.Liked.AnimesEligibleForRecommendation,
                    classifiedInput.Liked.MalUsername, classifiedInput.Liked.OkToRecommendPredicate),
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