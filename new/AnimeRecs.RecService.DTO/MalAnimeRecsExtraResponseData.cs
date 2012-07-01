using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.MalApi;

namespace AnimeRecs.RecService.DTO
{
    public class MalAnimeRecsExtraResponseData
    {
        public IList<MalAnimeRecsRecommender> Recommenders { get; set; }

        public MalAnimeRecsExtraResponseData()
        {
            ;
        }

        public MalAnimeRecsExtraResponseData(IList<MalAnimeRecsRecommender> recommenders)
        {
            Recommenders = recommenders;
        }
    }

    public class MalAnimeRecsRecommender
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public IList<MalAnimeRecsRecommenderRecommendation> Recs { get; set; }
        public double? Compatibility { get; set; }
        public double? CompatibilityLowEndpoint { get; set; }
        public double? CompatibilityHighEndpoint { get; set; }

        public MalAnimeRecsRecommender()
        {
            ;
        }

        public MalAnimeRecsRecommender(int userId, string username, IList<MalAnimeRecsRecommenderRecommendation> recs,
            double? compatibility, double? compatibilityLowEndpoint, double? compatibilityHighEndpoint)
        {
            UserId = userId;
            Username = username;
            Recs = recs;
            Compatibility = compatibility;
            CompatibilityLowEndpoint = compatibilityLowEndpoint;
            CompatibilityHighEndpoint = compatibilityHighEndpoint;
        }
    }

    public class MalAnimeRecsRecommenderRecommendation
    {
        public int MalAnimeId { get; set; }
        public bool? Liked { get; set; }
        public decimal? RecommenderScore { get; set; }
        public double AverageScore { get; set; }

        public MalAnimeRecsRecommenderRecommendation()
        {
            ;
        }

        public MalAnimeRecsRecommenderRecommendation(int malAnimeId, bool? liked, decimal? recommenderScore, double averageScore)
        {
            MalAnimeId = malAnimeId;
            Liked = liked;
            RecommenderScore = recommenderScore;
            AverageScore = averageScore;
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