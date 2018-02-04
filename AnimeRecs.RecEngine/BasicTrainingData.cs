using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Simple training data containing only a set of users that each have some sort of ratings.
    /// </summary>
    /// <typeparam name="TTrainingUserInput"></typeparam>
    public class BasicTrainingData<TTrainingUserInput> : IBasicTrainingData<TTrainingUserInput>
    {
        public IDictionary<int, TTrainingUserInput> Users { get; private set; }

        public BasicTrainingData(IDictionary<int, TTrainingUserInput> users)
        {
            Users = users;
        }
    }
}
