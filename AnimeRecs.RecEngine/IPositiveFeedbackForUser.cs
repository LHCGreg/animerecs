using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public interface IPositiveFeedbackForUser : IInputForUser
    {
        /// <summary>
        /// Item ids with positive feedback
        /// </summary>
        ICollection<int> Items { get; }
    }
}
