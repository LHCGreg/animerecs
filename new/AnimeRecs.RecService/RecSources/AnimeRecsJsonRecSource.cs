using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecEngine;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.RecSources
{
    internal class AnimeRecsJsonRecSource : TrainableJsonRecSource<MalAnimeRecsRecSource, MalAnimeRecsInput, MalAnimeRecsResults,
        RecEngine.AnimeRecsRecommendation, GetMalRecsResponse<DTO.AnimeRecsRecommendation, MalAnimeRecsExtraResponseData>, DTO.AnimeRecsRecommendation>
    {
        public AnimeRecsJsonRecSource(MalAnimeRecsRecSource underlyingRecSource)
            : base(underlyingRecSource)
        {
            ;
        }

        protected override MalAnimeRecsInput GetRecSourceInputFromRequest(MalUserListEntries animeList, GetMalRecsRequest recRequest, RecRequestCaster caster)
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

        protected override void SetSpecializedExtraResponseProperties(
            GetMalRecsResponse<DTO.AnimeRecsRecommendation, MalAnimeRecsExtraResponseData> response, MalAnimeRecsResults recResults)
        {
            response.Data = new MalAnimeRecsExtraResponseData(
                recommenders: recResults.Recommenders.Select(recommender => new DTO.MalAnimeRecsRecommender(
                    userId: recommender.UserId,
                    username: TrainingData.Users[recommender.UserId].MalUsername,
                    recs: recommender.AllRecommendations.Select(recommendedAnime => new DTO.MalAnimeRecsRecommenderRecommendation(
                        malAnimeId: recommendedAnime.MalAnimeId,
                        liked: recommender.RecsLiked.Contains(recommendedAnime) ? (bool?)true : recommender.RecsNotLiked.Contains(recommendedAnime) ? (bool?)false : null,
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
