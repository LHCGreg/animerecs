using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Can provide a set of recommendations given input for a user.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TRecommendation"></typeparam>
    public interface IRecommendationSource<in TInput, out TRecommendationResults, out TRecommendation>
        where TInput : IInputForUser
        where TRecommendationResults : IEnumerable<TRecommendation>
        where TRecommendation : IRecommendation
    {
        /// <summary>
        /// Tries to to get <paramref name="numRecommendationsToTryToGet"/> recommendations for the given user.
        /// The recommendation source may return fewer if it is not able to provide that many. The recommendation source should
        /// honor the ItemIsOkToRecommend() method of the input and avoid returning items that are not ok to recommend.
        /// </summary>
        /// <param name="inputForUser"></param>
        /// <param name="numRecommendationsToTryToGet"></param>
        /// <returns></returns>
        TRecommendationResults GetRecommendations(TInput inputForUser, int numRecommendationsToTryToGet);
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.
//
// AnimeRecs.RecEngine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.  If not, see <http://www.gnu.org/licenses/>.