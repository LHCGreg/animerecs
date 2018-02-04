using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Classifies input for a user into liked, unliked and other. Normal input should be either liked or unliked.
    /// "Other" is for anything else in the input needed to form the complete original input. For example, if the input includes
    /// items the user plans to watch but has not watched yet, that would go into "Other".
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public interface IUserInputClassifier<TInput>
    {
        ClassifiedUserInput<TInput> Classify(TInput inputForUser);
    }
}
