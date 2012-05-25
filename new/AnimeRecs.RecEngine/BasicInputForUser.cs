using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class BasicInputForUser : IBasicInputForUser
    {
        public IDictionary<int, float> Ratings { get; private set; }

        public BasicInputForUser()
        {
            Ratings = new Dictionary<int, float>();
        }

        public BasicInputForUser(IDictionary<int, float> ratings)
        {
            Ratings = ratings;
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            return !Ratings.ContainsKey(itemId);
        }

        public bool ContainsItem(int itemId)
        {
            return Ratings.ContainsKey(itemId);
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