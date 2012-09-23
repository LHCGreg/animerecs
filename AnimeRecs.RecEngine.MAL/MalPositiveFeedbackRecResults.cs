using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalPositiveFeedbackRecResults : IEnumerable<RatingPredictionRecommendation>
    {
        public IEnumerable<RatingPredictionRecommendation> Recommendations { get; private set; }
        public decimal TargetScoreUsed { get; private set; }

        public MalPositiveFeedbackRecResults(IEnumerable<RatingPredictionRecommendation> recommendations, decimal targetScoreUsed)
        {
            Recommendations = recommendations;
            TargetScoreUsed = targetScoreUsed;
        }

        public IEnumerator<RatingPredictionRecommendation> GetEnumerator()
        {
            return Recommendations.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.MAL.
//
// AnimeRecs.RecEngine.MAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine.MAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.MAL.  If not, see <http://www.gnu.org/licenses/>.