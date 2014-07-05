using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Input classifier for basic input.
    /// </summary>
    public class BasicInputForUserPercentageClassifier : PercentageRatingClassifier<BasicInputForUser>
    {
        public BasicInputForUserPercentageClassifier(double goodPercentage)
            : base(goodPercentage, GetBasicInputForUserSubset)
        {
            ;
        }

        private static BasicInputForUser GetBasicInputForUserSubset(BasicInputForUser user, ICollection<int> itemIds)
        {
            IDictionary<int, float> ratingsSubset = new Dictionary<int, float>();
            foreach(int itemId in itemIds)
            {
                ratingsSubset[itemId] = user.Ratings[itemId];
            }
            return new BasicInputForUser(ratingsSubset);
        }
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