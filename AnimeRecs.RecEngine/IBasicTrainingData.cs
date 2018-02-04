using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Training data that has a set of users.
    /// </summary>
    /// <typeparam name="TUserRatings"></typeparam>
    public interface IBasicTrainingData<TUserRatings>
    {
        /// <summary>
        /// Maps user ids to user data.
        /// </summary>
        IDictionary<int, TUserRatings> Users { get; }
    }
}
