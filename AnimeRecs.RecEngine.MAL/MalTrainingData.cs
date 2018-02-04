using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalTrainingData : IBasicTrainingData<MalUserListEntries>
    {
        public IDictionary<int, MalUserListEntries> Users { get; private set; }
        public IDictionary<int, MalAnime> Animes { get; private set; }

        public MalTrainingData()
        {
            Users = new Dictionary<int, MalUserListEntries>();
            Animes = new Dictionary<int, MalAnime>();
        }

        public MalTrainingData(IDictionary<int, MalUserListEntries> users, IDictionary<int, MalAnime> animes)
        {
            Users = users;
            Animes = animes;
        }

        public IBasicTrainingData<IBasicInputForUser> AsBasicTrainingData(int minEpisodesWatchedToCount, bool includeDropped)
        {
            Dictionary<int, IBasicInputForUser> basicUsers = new Dictionary<int, IBasicInputForUser>();
            foreach (int userId in Users.Keys)
            {
                basicUsers[userId] = Users[userId].AsBasicInput(minEpisodesWatchedToCount, includeDropped);
            }

            return new BasicTrainingData<IBasicInputForUser>(basicUsers);
        }

        public IBasicTrainingData<IPositiveFeedbackForUser> AsPositiveFeedback(IUserInputClassifier<MalUserListEntries> classifier)
        {
            Dictionary<int, IPositiveFeedbackForUser> basicUsers = new Dictionary<int, IPositiveFeedbackForUser>();
            foreach(int userId in Users.Keys)
            {
                basicUsers[userId] = Users[userId].AsPositiveFeedback(classifier);
            }

            return new BasicTrainingData<IPositiveFeedbackForUser>(basicUsers);
        }

        public override string ToString()
        {
            return string.Format("{0} users", Users.Count);
        }
    }
}
