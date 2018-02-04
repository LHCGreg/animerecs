using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalMostPopularRecSource
        : ITrainableRecSource<MalTrainingData, MalUserListEntries, IEnumerable<MostPopularRecommendation>, MostPopularRecommendation>
    {
        private MostPopularRecSource<IBasicTrainingData<IBasicInputForUser>, IBasicInputForUser>
            m_recommender = new MostPopularRecSource<IBasicTrainingData<IBasicInputForUser>, IBasicInputForUser>();

        public int MinEpisodesToCountIncomplete { get; private set; }
        public bool UseDropped { get; private set; }

        public MalMostPopularRecSource(int minEpisodesToCountIncomplete, bool useDropped)
        {
            MinEpisodesToCountIncomplete = minEpisodesToCountIncomplete;
            UseDropped = useDropped;
        }

        public void Train(MalTrainingData trainingData)
        {
            IBasicTrainingData<IBasicInputForUser> basicTrainingData =
                trainingData.AsBasicTrainingData(MinEpisodesToCountIncomplete, UseDropped);
            m_recommender.Train(basicTrainingData);
        }

        public IEnumerable<MostPopularRecommendation> GetRecommendations(MalUserListEntries inputForUser, int numRecommendationsToTryToGet)
        {
            return m_recommender.GetRecommendations(inputForUser, numRecommendationsToTryToGet);
        }

        public override string ToString()
        {
            return string.Format("MostPopular MinEpisodesToCountIncomplete={0}, UseDropped={1}",
                MinEpisodesToCountIncomplete, UseDropped);
        }
    }
}
