#if MYMEDIALITE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;
using MyMediaLite.ItemRecommendation;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.RecService.Registrations.RecSources
{
    [JsonRecSource(RecSourceTypes.BPRMF)]
    internal class BPRMFJsonRecSource : 
        TrainableJsonRecSource<MalMyMediaLiteItemRecommenderRecSource<BPRMF>, MalPositiveFeedbackInput, MalPositiveFeedbackRecResults,
        AnimeRecs.RecEngine.RatingPredictionRecommendation, GetMalRecsResponse<DTO.RatingPredictionRecommendation, MalPositiveFeedbackExtraResponseData>,
        DTO.RatingPredictionRecommendation>
    {
        public BPRMFJsonRecSource(LoadRecSourceRequest<BPRMFRecSourceParams> request)
            : base(CreateRecSourceFromRequest(request))
        {
            ;
        }

        private static MalMyMediaLiteItemRecommenderRecSource<BPRMF> CreateRecSourceFromRequest(LoadRecSourceRequest<BPRMFRecSourceParams> request)
        {
            BPRMF underlyingRecSource = new BPRMF();
            if (request.Params.BiasReg != null)
                underlyingRecSource.BiasReg = request.Params.BiasReg.Value;
            if (request.Params.LearnRate != null)
                underlyingRecSource.LearnRate = request.Params.LearnRate.Value;
            if (request.Params.NumFactors != null)
                underlyingRecSource.NumFactors = request.Params.NumFactors.Value;
            if (request.Params.NumIter != null)
                underlyingRecSource.NumIter = request.Params.NumIter.Value;
            if (request.Params.RegI != null)
                underlyingRecSource.RegI = request.Params.RegI.Value;
            if (request.Params.RegJ != null)
                underlyingRecSource.RegJ = request.Params.RegJ.Value;
            if (request.Params.RegU != null)
                underlyingRecSource.RegU = request.Params.RegU.Value;
            if (request.Params.UniformUserSampling != null)
                underlyingRecSource.UniformUserSampling = request.Params.UniformUserSampling.Value;
            if (request.Params.UpdateJ != null)
                underlyingRecSource.UpdateJ = request.Params.UpdateJ.Value;
            if (request.Params.WithReplacement != null)
                underlyingRecSource.WithReplacement = request.Params.WithReplacement.Value;

            MalMyMediaLiteItemRecommenderRecSource<BPRMF> recSource = new MalMyMediaLiteItemRecommenderRecSource<BPRMF>(
                recommender: underlyingRecSource,
                fractionConsideredRecommended: request.Params.FractionConsideredRecommended,
                minEpisodesToClassifyIncomplete: request.Params.MinEpisodesToClassifyIncomplete,
                minUsersToCountAnime: request.Params.MinUsersToCountAnime
            );

            return recSource;
        }
        
        protected override MalPositiveFeedbackInput GetRecSourceInputFromRequest(MalUserListEntries animeList, GetMalRecsRequest recRequest)
        {
            if (recRequest.TargetScore != null)
            {
                return new MalPositiveFeedbackInput(animeList, recRequest.TargetScore.Value);
            }
            else
            {
                return new MalPositiveFeedbackInput(animeList, recRequest.TargetFraction.Value);
            }
        }

        protected override void SetSpecializedRecommendationProperties(DTO.RatingPredictionRecommendation dtoRec, RecEngine.RatingPredictionRecommendation engineRec)
        {
            dtoRec.PredictedRating = engineRec.PredictedRating;
        }

        protected override void SetSpecializedExtraResponseProperties(
            GetMalRecsResponse<DTO.RatingPredictionRecommendation, MalPositiveFeedbackExtraResponseData> response,
            MalPositiveFeedbackRecResults recResults)
        {
            response.Data = new MalPositiveFeedbackExtraResponseData(targetScoreUsed: recResults.TargetScoreUsed);
        }

        protected override string RecommendationType { get { return RecommendationTypes.RatingPrediction; } }
        public override string RecSourceType { get { return RecSourceTypes.BPRMF; } }
    }
}

#endif
