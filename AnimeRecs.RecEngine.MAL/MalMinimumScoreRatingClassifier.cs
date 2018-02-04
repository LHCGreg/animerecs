using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MalApi;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalMinimumScoreRatingClassifier : IUserInputClassifier<MalUserListEntries>
    {
        public decimal MinimumGoodScore { get; private set; }
        public int MinEpisodesToClassifyIncomplete { get; private set; }

        public MalMinimumScoreRatingClassifier(decimal minimumGoodScore, int minEpisodesToClassifyIncomplete)
        {
            MinimumGoodScore = minimumGoodScore;
            MinEpisodesToClassifyIncomplete = minEpisodesToClassifyIncomplete;
        }

        public ClassifiedUserInput<MalUserListEntries> Classify(MalUserListEntries inputForUser)
        {
            Dictionary<int, MalListEntry> likedAnimes = new Dictionary<int, MalListEntry>();
            Dictionary<int, MalListEntry> unlikedAnimes = new Dictionary<int, MalListEntry>();
            Dictionary<int, MalListEntry> otherAnimes = new Dictionary<int, MalListEntry>();

            // Dropped anime is automatically considered unliked.
            // All other anime that is completed or has > N episodes seen gets classified by rating
            // Everything else goes into Other.

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
                    if (entry.Rating.Value >= MinimumGoodScore)
                    {
                        likedAnimes[animeId] = entry;
                    }
                    else
                    {
                        unlikedAnimes[animeId] = entry;
                    }
                }
                else if (entry.NumEpisodesWatched > MinEpisodesToClassifyIncomplete && entry.Rating != null)
                {
                    if (entry.Rating.Value >= MinimumGoodScore)
                    {
                        likedAnimes[animeId] = entry;
                    }
                    else
                    {
                        unlikedAnimes[animeId] = entry;
                    }
                }
                else
                {
                    otherAnimes[animeId] = entry;
                }
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
            return string.Format("MinimumGoodScore = {0}, MinEpisodesToClassifyIncomplete = {1}", MinimumGoodScore, MinEpisodesToClassifyIncomplete);
        }
    }
}
