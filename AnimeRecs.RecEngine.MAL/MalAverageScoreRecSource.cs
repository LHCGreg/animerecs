using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalAverageScoreRecSource
        : ITrainableRecSource<MalTrainingData, MalUserListEntries, IEnumerable<AverageScoreRecommendation>, AverageScoreRecommendation>
    {
        private AverageScoreRecSource<IBasicTrainingData<IBasicInputForUser>, IBasicInputForUser>
            m_recommender = new AverageScoreRecSource<IBasicTrainingData<IBasicInputForUser>, IBasicInputForUser>();

        public int MinEpisodesToCountIncomplete { get; private set; }
        public bool UseDropped { get; private set; }
        public int MinUsersToCountAnime { get; private set; }
        
        public MalAverageScoreRecSource(int minEpisodesToCountIncomplete, bool useDropped, int minUsersToCountAnime)
        {
            MinEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            UseDropped = useDropped;
            MinUsersToCountAnime = minUsersToCountAnime;
        }

        public void Train(MalTrainingData trainingData)
        {
            IBasicTrainingData<IBasicInputForUser> basicTrainingData =
                trainingData.AsBasicTrainingData(MinEpisodesToCountIncomplete, UseDropped);

            IBasicTrainingData<IBasicInputForUser> filteredTrainingData = FilterHelpers.RemoveItemsWithFewUsers(basicTrainingData, MinUsersToCountAnime);
            m_recommender.Train(filteredTrainingData);
        }

        public IEnumerable<AverageScoreRecommendation> GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            return m_recommender.GetRecommendations(inputForUser, numRecommendationsToTryToGet);
        }

        public override string ToString()
        {
            return string.Format("AverageScore MinEpisodesToCountIncomplete={0} UseDropped={1} MinUsersToCountAnime={2}",
                MinEpisodesToCountIncomplete, UseDropped, MinUsersToCountAnime);
        }
    }
}
