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
                HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation> recsLiked = new HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation>();
                HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation> recsNotLiked = new HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation>();
                HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation> recsInconclusive = new HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation>();
                HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation> recsNotInCommon = new HashSet<RecEngine.MAL.MalAnimeRecsRecommenderRecommendation>();
                
                foreach (DTO.MalAnimeRecsRecommenderRecommendation rec in dtoRecommender.Recs)
                {
                    switch (rec.Judgment)
                    {
                        case AnimeRecsRecommendationJudgment.Liked:
                            recsLiked.Add(new RecEngine.MAL.MalAnimeRecsRecommenderRecommendation(rec.MalAnimeId, rec.RecommenderScore, rec.AverageScore));
                            break;
                        case AnimeRecsRecommendationJudgment.NotLiked:
                            recsNotLiked.Add(new RecEngine.MAL.MalAnimeRecsRecommenderRecommendation(rec.MalAnimeId, rec.RecommenderScore, rec.AverageScore));
                            break;
                        case AnimeRecsRecommendationJudgment.Inconclusive:
                            recsInconclusive.Add(new RecEngine.MAL.MalAnimeRecsRecommenderRecommendation(rec.MalAnimeId, rec.RecommenderScore, rec.AverageScore));
                            break;
                        case AnimeRecsRecommendationJudgment.NotInCommon:
                            recsNotInCommon.Add(new RecEngine.MAL.MalAnimeRecsRecommenderRecommendation(rec.MalAnimeId, rec.RecommenderScore, rec.AverageScore));
                            break;
                    }
                }

                recommenders.Add(new MalAnimeRecsRecommenderUser(
                    userId: dtoRecommender.UserId,
                    username: dtoRecommender.Username,
                    recsLiked: recsLiked,
                    recsNotLiked: recsNotLiked,
                    recsInconclusive: recsInconclusive,
                    recsNotInCommon: recsNotInCommon,
                    compatibility: dtoRecommender.Compatibility,
                    compatibilityLowEndpoint: dtoRecommender.CompatibilityLowEndpoint,
                    compatibilityHighEndpoint: dtoRecommender.CompatibilityHighEndpoint
                ));
            }

            return new RecEngine.MAL.MalAnimeRecsResults(recommendations, recommenders, response.Data.TargetScoreUsed);
        }
    }
}
