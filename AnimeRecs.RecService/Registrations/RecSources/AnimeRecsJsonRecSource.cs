using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.Registrations.RecSources
{
    [JsonRecSource(RecSourceTypes.AnimeRecs)]
    internal class AnimeRecsJsonRecSource : TrainableJsonRecSource<MalAnimeRecsRecSource, MalAnimeRecsInput, MalAnimeRecsResults,
        RecEngine.AnimeRecsRecommendation, GetMalRecsResponse<DTO.AnimeRecsRecommendation, MalAnimeRecsExtraResponseData>, DTO.AnimeRecsRecommendation>
    {
        public AnimeRecsJsonRecSource(LoadRecSourceRequest<AnimeRecsRecSourceParams> request)
            : base(CreateRecSourceFromRequest(request))
        {
            ;
        }

        private static MalAnimeRecsRecSource CreateRecSourceFromRequest(LoadRecSourceRequest<AnimeRecsRecSourceParams> request)
        {
            return new MalAnimeRecsRecSource(
                numRecommendersToUse: request.Params.NumRecommendersToUse,
                fractionConsideredRecommended: request.Params.FractionConsideredRecommended,
                minEpisodesToClassifyIncomplete: request.Params.MinEpisodesToClassifyIncomplete
            );
        }

        protected override MalAnimeRecsInput GetRecSourceInputFromRequest(MalUserListEntries animeList, GetMalRecsRequest recRequest)
        {
            if (recRequest.TargetScore != null)
            {
                return new MalAnimeRecsInput(animeList, targetScore: recRequest.TargetScore.Value);
            }
            else
            {
                return new MalAnimeRecsInput(animeList, targetFraction: (double)recRequest.TargetFraction.Value);
            }
        }

        protected override void SetSpecializedRecommendationProperties(DTO.AnimeRecsRecommendation dtoRec, RecEngine.AnimeRecsRecommendation engineRec)
        {
            dtoRec.RecommenderUserId = engineRec.RecommenderUserId;
        }

        protected override string RecommendationType { get { return RecommendationTypes.AnimeRecs; } }
        public override string RecSourceType { get { return RecSourceTypes.AnimeRecs; } }

        protected override void SetSpecializedExtraResponseProperties(
            GetMalRecsResponse<DTO.AnimeRecsRecommendation, MalAnimeRecsExtraResponseData> response, MalAnimeRecsResults recResults)
        {
            response.Data = new MalAnimeRecsExtraResponseData(
                targetScoreUsed: recResults.TargetScoreUsed,
                recommenders: recResults.Recommenders.Select(recommender => new DTO.MalAnimeRecsRecommender(
                    userId: recommender.UserId,
                    username: UsernamesByUserId[recommender.UserId],
                    recs: recommender.AllRecommendations.Select(recommendedAnime => new DTO.MalAnimeRecsRecommenderRecommendation(
                        malAnimeId: recommendedAnime.MalAnimeId,
                        judgment: recommender.RecsLiked.Contains(recommendedAnime) ? DTO.AnimeRecsRecommendationJudgment.Liked
                                : recommender.RecsNotLiked.Contains(recommendedAnime) ? DTO.AnimeRecsRecommendationJudgment.NotLiked
                                : recommender.RecsInconclusive.Contains(recommendedAnime) ? DTO.AnimeRecsRecommendationJudgment.Inconclusive
                                : AnimeRecsRecommendationJudgment.NotInCommon,
                        recommenderScore: recommendedAnime.RecommenderScore,
                        averageScore: recommendedAnime.AverageScore
                    )).ToList(),
                    compatibility: recommender.Compatibility,
                    compatibilityLowEndpoint: recommender.CompatibilityLowEndpoint,
                    compatibilityHighEndpoint: recommender.CompatibilityHighEndpoint
                )).ToList()
            );
        }

        protected override HashSet<int> GetExtraAnimesToReturn(MalAnimeRecsResults recResults)
        {
            HashSet<int> recommendedIds = new HashSet<int>();
            
            foreach (RecEngine.MAL.MalAnimeRecsRecommenderUser recommender in recResults.Recommenders)
            {
                foreach (RecEngine.MAL.MalAnimeRecsRecommenderRecommendation recommendedAnime in recommender.AllRecommendations)
                {
                    recommendedIds.Add(recommendedAnime.MalAnimeId);
                }
            }

            return recommendedIds;
        }
    }
}
