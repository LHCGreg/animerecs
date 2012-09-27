using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MalApi;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalPercentageRatingClassifier : IUserInputClassifier<MalUserListEntries>
    {
        public double GoodFraction { get; private set; }
        public int MinEpisodesToClassifyIncomplete { get; private set; }

        public MalPercentageRatingClassifier(double goodFraction, int minEpisodesToClassifyIncomplete)
        {
            GoodFraction = goodFraction;
            MinEpisodesToClassifyIncomplete = minEpisodesToClassifyIncomplete;
        }
        
        public ClassifiedUserInput<MalUserListEntries> Classify(MalUserListEntries inputForUser)
        {
            Dictionary<int, MalListEntry> likedAnimes = new Dictionary<int, MalListEntry>();
            Dictionary<int, MalListEntry> unlikedAnimes = new Dictionary<int, MalListEntry>();
            Dictionary<int, MalListEntry> otherAnimes = new Dictionary<int, MalListEntry>();

            // Dropped anime is automatically considered unliked.
            // All other anime that is completed or has > N episodes seen gets percentage-classified.
            List<KeyValuePair<int, MalListEntry>> animesEligibleForPercentageClassification = new List<KeyValuePair<int, MalListEntry>>();

            foreach (KeyValuePair<int, MalListEntry> animeIdAndEntry in inputForUser.Entries)
            {
                int animeId = animeIdAndEntry.Key;
                MalListEntry entry = animeIdAndEntry.Value;

                if (entry.Status == CompletionStatus.Dropped)
                {
                    unlikedAnimes[animeId] = entry;
                }
                else if (entry.Status == CompletionStatus.Completed && entry.Rating != null)
                {
                    animesEligibleForPercentageClassification.Add(animeIdAndEntry);
                }
                else if (entry.NumEpisodesWatched > MinEpisodesToClassifyIncomplete && entry.Rating != null)
                {
                    animesEligibleForPercentageClassification.Add(animeIdAndEntry);
                }
                else
                {
                    otherAnimes[animeId] = entry;
                }
            }

            PercentageSplit<KeyValuePair<int, MalListEntry>> percentageClassified = RecUtils.SplitByPercentage(
                animesEligibleForPercentageClassification, GoodFraction,
                (animeIdAndEntry1, animeIdAndEntry2) => animeIdAndEntry1.Value.Rating.Value.CompareTo(animeIdAndEntry2.Value.Rating.Value));

            foreach (KeyValuePair<int, MalListEntry> unlikedAnimeIdAndEntry in percentageClassified.LowerPart)
            {
                unlikedAnimes[unlikedAnimeIdAndEntry.Key] = unlikedAnimeIdAndEntry.Value;
            }

            foreach (KeyValuePair<int, MalListEntry> likedAnimeIdAndEntry in percentageClassified.UpperPart)
            {
                likedAnimes[likedAnimeIdAndEntry.Key] = likedAnimeIdAndEntry.Value;
            }

            return new ClassifiedUserInput<MalUserListEntries>(
                liked: new MalUserListEntries(ratings: likedAnimes, animes: inputForUser.AnimesEligibleForRecommendation,
                    malUsername: inputForUser.MalUsername, okToRecommendPredicate: inputForUser.OkToRecommendPredicate),
                notLiked: new MalUserListEntries(ratings: unlikedAnimes, animes: inputForUser.AnimesEligibleForRecommendation,
                    malUsername: inputForUser.MalUsername, okToRecommendPredicate: inputForUser.OkToRecommendPredicate),
                other: new MalUserListEntries(ratings: otherAnimes, animes: inputForUser.AnimesEligibleForRecommendation,
                    malUsername: inputForUser.MalUsername, okToRecommendPredicate: inputForUser.OkToRecommendPredicate)
            );
        }

        public override string ToString()
        {
            return string.Format("GoodFraction = {0:P}, MinEpisodesToClassifyIncomplete = {1}", GoodFraction, MinEpisodesToClassifyIncomplete);
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