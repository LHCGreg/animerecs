﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    /// <summary>
    /// Classifies basic user input into liked and unliked, with empty "Other". The cutoff between liked and unliked is placed
    /// so as to make the liked portion as close as possible to a given percentage of the input.
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    public class PercentageRatingClassifier<TInput> : IUserInputClassifier<TInput>
        where TInput : IBasicInputForUser, new()
    {
        public double GoodPercentage { get; private set; }
        private Func<TInput, ICollection<int>, TInput> m_ratingsTrimmingFunc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goodPercentage"></param>
        /// <param name="ratingsTrimmingFunc">Takes input for a user and a set of item ids and returns input for the user with only those item ids.</param>
        public PercentageRatingClassifier(double goodPercentage, Func<TInput, ICollection<int>, TInput> ratingsTrimmingFunc)
        {
            GoodPercentage = goodPercentage;
            m_ratingsTrimmingFunc = ratingsTrimmingFunc;
        }

        public ClassifiedUserInput<TInput> Classify(TInput allRatings)
        {
            List<int> itemIds = allRatings.Ratings.Keys.ToList();
            PercentageSplit<int> divided = RecUtils.SplitByPercentage(itemIds, GoodPercentage, (x, y) => allRatings.Ratings[x].CompareTo(allRatings.Ratings[y]));

            return new ClassifiedUserInput<TInput>(
                liked: m_ratingsTrimmingFunc(allRatings, divided.UpperPart),
                notLiked: m_ratingsTrimmingFunc(allRatings, divided.LowerPart),
                other: new TInput()
            );
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