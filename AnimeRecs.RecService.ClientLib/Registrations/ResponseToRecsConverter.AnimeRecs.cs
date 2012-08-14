using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.RecService.ClientLib.Registrations
{
    internal partial class ResponseToRecsConverter :
        IResponseToRecsConverter<GetMalRecsResponse<DTO.AnimeRecsRecommendation, DTO.MalAnimeRecsExtraResponseData>>
    {
        // Take in type derived from GetMalRecsResponse, return IEnumerable<IRecommendation>
        IEnumerable<RecEngine.IRecommendation> IResponseToRecsConverter<GetMalRecsResponse<DTO.AnimeRecsRecommendation, DTO.MalAnimeRecsExtraResponseData>>.ConvertResponseToRecommendations(GetMalRecsResponse<DTO.AnimeRecsRecommendation, DTO.MalAnimeRecsExtraResponseData> response)
        {
            List<RecEngine.AnimeRecsRecommendation> recommendations = new List<RecEngine.AnimeRecsRecommendation>();
            foreach (DTO.AnimeRecsRecommendation dtoRec in response.Recommendations)
            {
                recommendations.Add(new RecEngine.AnimeRecsRecommendation(dtoRec.RecommenderUserId, itemId: dtoRec.MalAnimeId));
            }

            List<MalAnimeRecsRecommenderUser> recommenders = new List<MalAnimeRecsRecommenderUser>();
            foreach (DTO.MalAnimeRecsRecommender dtoRecommender in response.Data.Recommenders)
            {
                HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation> recsLiked = new HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation>(
                        dtoRecommender.Recs.Where(rec => rec.Liked.HasValue && rec.Liked.Value == true)
                        .Select(rec => new RecEngine.MAL.MalAnimeRecsRecommenderRecommendation(rec.MalAnimeId, rec.RecommenderScore, rec.AverageScore)));

                HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation> recsNotLiked = new HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation>(
                        dtoRecommender.Recs.Where(rec => rec.Liked.HasValue && rec.Liked.Value == false)
                        .Select(rec => new RecEngine.MAL.MalAnimeRecsRecommenderRecommendation(rec.MalAnimeId, rec.RecommenderScore, rec.AverageScore)));

                recommenders.Add(new MalAnimeRecsRecommenderUser(
                    userId: dtoRecommender.UserId,
                    username: dtoRecommender.Username,
                    recsLiked: recsLiked,
                    recsNotLiked: recsNotLiked,
                    allRecommendations: new HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation>(
                        dtoRecommender.Recs.Select(rec => new RecEngine.MAL.MalAnimeRecsRecommenderRecommendation(rec.MalAnimeId, rec.RecommenderScore, rec.AverageScore))),
                    compatibility: dtoRecommender.Compatibility,
                    compatibilityLowEndpoint: dtoRecommender.CompatibilityLowEndpoint,
                    compatibilityHighEndpoint: dtoRecommender.CompatibilityHighEndpoint
                ));
            }

            return new RecEngine.MAL.MalAnimeRecsResults(recommendations, recommenders, response.Data.TargetScoreUsed);
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