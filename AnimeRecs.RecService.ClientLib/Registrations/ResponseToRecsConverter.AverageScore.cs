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
