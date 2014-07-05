using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public static class RecUtils
    {
        /// <summary>
        /// Splits by percentage with at least one item in the upper portion and with equal items all in one portion or the other.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="things"></param>
        /// <param name="fractionOfUpper"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static PercentageSplit<T> SplitByPercentage<T>(IList<T> things, double fractionOfUpper, Comparison<T> comparer)
        {
            if (things.Count == 0)
            {
                return new PercentageSplit<T>()
                {
                    LowerPart = new List<T>(),
                    UpperPart = new List<T>()
                };
            }

            if (fractionOfUpper <= 0)
            {
                return new PercentageSplit<T>()
                {
                    LowerPart = new List<T>(),
                    UpperPart = new List<T>(things) // Upper part is guaranteed to be non-empty
                };
            }

            List<T> sortedThings = new List<T>(things);
            sortedThings.Sort((x, y) => -comparer(x, y)); // Sort in descending order

            int targetSizeOfUpperPortion = (int)Math.Ceiling(sortedThings.Count * fractionOfUpper);

            int bestCutoffIndex = 0;

            // 777777766666655554443221
            //          ^      ^   ^
            //

            // index <= cutoff is in upper
            // An index is a real cutoff if index to the right is not equal
            // -1 is not a real cutoff because we want to guarantee at least one item in the upper portion
            // things.Count - 1 is a real cutoff

            T valueOfLastThing = default(T);
            for (int i = 0; i < sortedThings.Count; i++)
            {
                if (i > 0 && comparer(sortedThings[i], valueOfLastThing) != 0)
                {
                    int candidateCutoffIndex = i - 1;
                    int numItemsTakenWithCandidate = i;
                    int differenceFromTargetWithCandidate = Math.Abs(numItemsTakenWithCandidate - targetSizeOfUpperPortion);

                    int numItemsTakenWithBest = bestCutoffIndex + 1;
                    int differenceFromTargetWithBest = Math.Abs(numItemsTakenWithBest - targetSizeOfUpperPortion);

                    if (differenceFromTargetWithCandidate < differenceFromTargetWithBest)
                    {
                        bestCutoffIndex = candidateCutoffIndex;
                    }
                }

                valueOfLastThing = sortedThings[i];
            }

            int lastCandidateCutoffIndex = sortedThings.Count - 1;
            int lastNumItemsTakenWithCandidate = sortedThings.Count;
            int lastDifferenceFromTargetWithCandidate = Math.Abs(lastNumItemsTakenWithCandidate - targetSizeOfUpperPortion);

            int lastNumItemsTakenWithBest = bestCutoffIndex + 1;
            int lastDifferenceFromTargetWithBest = Math.Abs(lastNumItemsTakenWithBest - targetSizeOfUpperPortion);

            if (lastDifferenceFromTargetWithCandidate < lastDifferenceFromTargetWithBest)
            {
                bestCutoffIndex = lastCandidateCutoffIndex;
            }

            List<T> upperPortion = new List<T>();
            for (int i = 0; i <= bestCutoffIndex; i++)
            {
                upperPortion.Add(sortedThings[i]);
            }

            List<T> lowerPortion = new List<T>();
            for (int i = bestCutoffIndex + 1; i < things.Count; i++)
            {
                lowerPortion.Add(sortedThings[i]);
            }

            return new PercentageSplit<T>()
            {
                UpperPart = upperPortion,
                LowerPart = lowerPortion
            };
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