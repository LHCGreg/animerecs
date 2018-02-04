using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Input for a user consisting of a set of item ratings.
    /// </summary>
    public interface IBasicInputForUser : IInputForUser
    {
        /// <summary>
        /// Mapping of item id to item rating
        /// </summary>
        IDictionary<int, float> Ratings { get; }
    }
}
