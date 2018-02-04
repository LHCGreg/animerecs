using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.ClientLib.Registrations
{
    internal partial class ResponseToRecsConverter :
            IResponseToRecsConverter<GetMalRecsResponse<DTO.MostPopularRecommendation>>
    {
        // Take in type derived from GetMalRecsResponse, return IEnumerable<IRecommendation>
        IEnumerable<RecEngine.IRecommendation> IResponseToRecsConverter<GetMalRecsResponse<MostPopularRecommendation>>.ConvertResponseToRecommendations(GetMalRecsResponse<MostPopularRecommendation> response)
        {
            List<RecEngine.MostPopularRecommendation> recommendations = new List<RecEngine.MostPopularRecommendation>();
            foreach (DTO.MostPopularRecommendation dtoRec in response.Recommendations)
            {
                recommendations.Add(new AnimeRecs.RecEngine.MostPopularRecommendation(
                    itemId: dtoRec.MalAnimeId,
                    popularityRank: dtoRec.PopularityRank,
                    numRatings: dtoRec.NumRatings
                ));
            }
            return recommendations;
        }
    }
}
