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
