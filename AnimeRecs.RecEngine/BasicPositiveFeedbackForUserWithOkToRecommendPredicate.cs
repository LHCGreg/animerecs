﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class BasicPositiveFeedbackForUserWithOkToRecommendPredicate : IPositiveFeedbackForUser
    {
        public ICollection<int> Items { get; private set; }

        private Predicate<int> m_okToRecommendItemFunc;

        public BasicPositiveFeedbackForUserWithOkToRecommendPredicate(ICollection<int> items, Predicate<int> okToRecommendItemFunc)
        {
            Items = items;
            m_okToRecommendItemFunc = okToRecommendItemFunc;
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            return !Items.Contains(itemId) && m_okToRecommendItemFunc(itemId);
        }

        public bool ContainsItem(int itemId)
        {
            return Items.Contains(itemId);
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