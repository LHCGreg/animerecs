using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.ClientLib.Registrations
{
    internal partial class ResponseToRecsConverter :
        IResponseToRecsConverter<GetMalRecsResponse<DTO.AverageScoreRecommendation>>
    {
        // Take in type derived from GetMalRecsResponse, return IEnumerable<IRecommendation>
        IEnumerable<RecEngine.IRecommendation> IResponseToRecsConverter<GetMalRecsResponse<AverageScoreRecommendation>>.ConvertResponseToRecommendations(GetMalRecsResponse<AverageScoreRecommendation> response)
        {
            List<RecEngine.AverageScoreRecommendation> recommendations = new List<RecEngine.AverageScoreRecommendation>();
            foreach (DTO.AverageScoreRecommendation dtoRec in response.Recommendations)
            {
                recommendations.Add(new AnimeRecs.RecEngine.AverageScoreRecommendation(dtoRec.MalAnimeId, dtoRec.NumRatings, dtoRec.AverageScore));
            }
            return recommendations;
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