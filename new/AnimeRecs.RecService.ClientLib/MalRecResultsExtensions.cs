using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.ClientLib
{
    public static class MalRecResultsExtensions
    {
        public static MalRecResults<IEnumerable<RecEngine.AverageScoreRecommendation>> AsAverageScoreResults(this MalRecResults<IEnumerable<IRecommendation>> basicResults)
        {
            if (basicResults.RecommendationType.Equals(RecommendationTypes.AverageScore, StringComparison.OrdinalIgnoreCase))
            {
                IEnumerable<RecEngine.AverageScoreRecommendation> results = (IEnumerable<RecEngine.AverageScoreRecommendation>)basicResults.Results;
                return new MalRecResults<IEnumerable<RecEngine.AverageScoreRecommendation>>(results, basicResults.AnimeInfo, basicResults.RecommendationType);
            }
            else
            {
                return null;
            }
        }

        public static MalRecResults<RecEngine.MAL.MalAnimeRecsResults> AsAnimeRecsResults(this MalRecResults<IEnumerable<IRecommendation>> basicResults)
        {
            if (basicResults.RecommendationType.Equals(RecommendationTypes.AnimeRecs, StringComparison.OrdinalIgnoreCase))
            {
                RecEngine.MAL.MalAnimeRecsResults results = (RecEngine.MAL.MalAnimeRecsResults)basicResults.Results;
                return new MalRecResults<RecEngine.MAL.MalAnimeRecsResults>(results, basicResults.AnimeInfo, basicResults.RecommendationType);
            }
            else
            {
                return null;
            }
        }

        public static MalRecResults<IEnumerable<AnimeRecs.RecEngine.MostPopularRecommendation>> AsMostPopularResults(this MalRecResults<IEnumerable<IRecommendation>> basicResults)
        {
            if (basicResults.RecommendationType.Equals(RecommendationTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
            {
                IEnumerable<AnimeRecs.RecEngine.MostPopularRecommendation> results = (IEnumerable<AnimeRecs.RecEngine.MostPopularRecommendation>)basicResults.Results;
                return new MalRecResults<IEnumerable<RecEngine.MostPopularRecommendation>>(results, basicResults.AnimeInfo, basicResults.RecommendationType);
            }
            else
            {
                return null;
            }
        }

        public static MalRecResults<IEnumerable<AnimeRecs.RecEngine.RatingPredictionRecommendation>> AsRatingPredictionResults(this MalRecResults<IEnumerable<IRecommendation>> basicResults)
        {
            if (basicResults.RecommendationType.Equals(RecommendationTypes.RatingPrediction, StringComparison.OrdinalIgnoreCase))
            {
                IEnumerable<AnimeRecs.RecEngine.RatingPredictionRecommendation> results = (IEnumerable<AnimeRecs.RecEngine.RatingPredictionRecommendation>)basicResults.Results;
                return new MalRecResults<IEnumerable<RecEngine.RatingPredictionRecommendation>>(results, basicResults.AnimeInfo, basicResults.RecommendationType);
            }
            else
            {
                return null;
            }
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.ClientLib.
//
// AnimeRecs.RecService.ClientLib is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.ClientLib is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.ClientLib.  If not, see <http://www.gnu.org/licenses/>.