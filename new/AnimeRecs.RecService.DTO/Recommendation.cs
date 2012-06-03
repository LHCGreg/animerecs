using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.MalApi;

namespace AnimeRecs.RecService.DTO
{
    public class Recommendation
    {
        public int MalAnimeId { get; set; }
        public string Title { get; set; }
        public MalAnimeType MalAnimeType { get; set; }

        public Recommendation()
        {
            ;
        }

        public Recommendation(int malAnimeId, string title, MalAnimeType malAnimeType)
        {
            MalAnimeId = malAnimeId;
            Title = title;
            MalAnimeType = malAnimeType;
        }
    }

    public class AverageScoreRecommendation : Recommendation
    {
        public int NumRatings { get; set; }
        public float AverageScore { get; set; }

        public AverageScoreRecommendation()
        {
            ;
        }

        public AverageScoreRecommendation(int malAnimeId, string title, MalAnimeType malAnimeType, int numRatings, float averageScore)
            : base(malAnimeId: malAnimeId, title: title, malAnimeType: malAnimeType)
        {
            NumRatings = numRatings;
            AverageScore = averageScore;
        }
    }
    public class MostPopularRecommendation : Recommendation
    {
        public int PopularityRank { get; set; }
        public int NumRatings { get; set; }

        public MostPopularRecommendation()
        {
            ;
        }

        public MostPopularRecommendation(int malAnimeId, string title, MalAnimeType malAnimeType, int popularityRank, int numRatings)
            : base(malAnimeId: malAnimeId, title: title, malAnimeType: malAnimeType)
        {
            PopularityRank = popularityRank;
            NumRatings = numRatings;
        }
    }
}

// Copyright (C) 2012 Greg Najda
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