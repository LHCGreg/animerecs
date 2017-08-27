using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService.DTO
{
    [JsonClass]
    public class Recommendation
    {
        public int MalAnimeId { get; set; }

        public Recommendation()
        {
            ;
        }

        public Recommendation(int malAnimeId)
        {
            MalAnimeId = malAnimeId;
        }
    }

    [JsonClass]
    public class AverageScoreRecommendation : Recommendation
    {
        public int NumRatings { get; set; }
        public float AverageScore { get; set; }

        public AverageScoreRecommendation()
        {
            ;
        }

        public AverageScoreRecommendation(int malAnimeId, int numRatings, float averageScore)
            : base(malAnimeId)
        {
            NumRatings = numRatings;
            AverageScore = averageScore;
        }
    }

    [JsonClass]
    public class MostPopularRecommendation : Recommendation
    {
        public int PopularityRank { get; set; }
        public int NumRatings { get; set; }

        public MostPopularRecommendation()
        {
            ;
        }

        public MostPopularRecommendation(int malAnimeId, int popularityRank, int numRatings)
            : base(malAnimeId)
        {
            PopularityRank = popularityRank;
            NumRatings = numRatings;
        }
    }

    [JsonClass]
    public class AnimeRecsRecommendation : Recommendation
    {
        public int RecommenderUserId { get; set; }

        public AnimeRecsRecommendation()
        {
            ;
        }

        public AnimeRecsRecommendation(int malAnimeId, int recommenderUserId)
            : base(malAnimeId)
        {
            RecommenderUserId = recommenderUserId;
        }
    }

    [JsonClass]
    public class RatingPredictionRecommendation : Recommendation
    {
        public double PredictedRating { get; set; }

        public RatingPredictionRecommendation()
        {
            ;
        }

        public RatingPredictionRecommendation(int malAnimeId, double predictedRating)
            : base(malAnimeId)
        {
            PredictedRating = predictedRating;
        }
    }
}

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.RecService.DTO.
//
// AnimeRecs.RecService.DTO is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.DTO is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.DTO.  If not, see <http://www.gnu.org/licenses/>.