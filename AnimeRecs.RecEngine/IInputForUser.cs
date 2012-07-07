using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public interface IInputForUser
    {
        /// <summary>
        /// Returns true if the item is ok to return as a recommendation. The most obvious filter is to check
        /// if the user has already seen the given item. This is a good place to put domain-specific filters.
        /// For example, not recommending a sequel if the user has not seen the original.
        /// 
        /// Recommendation sources should consult this function for each potential recommendation and honor the result.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        bool ItemIsOkToRecommend(int itemId);

        /// <summary>
        /// Returns true if the input contains the given item.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        bool ContainsItem(int itemId);
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