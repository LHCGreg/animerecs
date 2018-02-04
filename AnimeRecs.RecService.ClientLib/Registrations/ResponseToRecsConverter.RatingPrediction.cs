using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.ClientLib.Registrations
{
    internal partial class ResponseToRecsConverter :
            IResponseToRecsConverter<GetMalRecsResponse<DTO.RatingPredictionRecommendation>>
    {
        IEnumerable<RecEngine.IRecommendation> IResponseToRecsConverter<GetMalRecsResponse<RatingPredictionRecommendation>>.ConvertResponseToRecommendations(GetMalRecsResponse<RatingPredictionRecommendation> response)
        {
            List<RecEngine.RatingPredictionRecommendation> recommendations = new List<RecEngine.RatingPredictionRecommendation>();
            foreach (DTO.RatingPredictionRecommendation dtoRec in response.Recommendations)
            {
                recommendations.Add(new RecEngine.RatingPredictionRecommendation(
                    itemId: dtoRec.MalAnimeId,
                    predictedRating: dtoRec.PredictedRating
                ));
            }

            return recommendations;
        }
    }
}
