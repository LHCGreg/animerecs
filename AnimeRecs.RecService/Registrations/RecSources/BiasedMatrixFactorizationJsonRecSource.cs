#if MYMEDIALITE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using MyMediaLite.RatingPrediction;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.Registrations.RecSources
{
    [JsonRecSource(RecSourceTypes.BiasedMatrixFactorization)]
    internal class BiasedMatrixFactorizationJsonRecSource :
        TrainableJsonRecSource<MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization>, MalUserListEntries,
        IEnumerable<RecEngine.RatingPredictionRecommendation>, RecEngine.RatingPredictionRecommendation, GetMalRecsResponse<DTO.RatingPredictionRecommendation>,
        DTO.RatingPredictionRecommendation>
    {
        public BiasedMatrixFactorizationJsonRecSource(LoadRecSourceRequest<BiasedMatrixFactorizationRecSourceParams> request)
            : base(CreateRecSourceFromRequest(request))
        {
            ;
        }

        private static MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization> CreateRecSourceFromRequest(LoadRecSourceRequest<BiasedMatrixFactorizationRecSourceParams> request)
        {
            BiasedMatrixFactorization underlyingRecSource = new BiasedMatrixFactorization();
            if (request.Params.BiasLearnRate != null)
                underlyingRecSource.BiasLearnRate = request.Params.BiasLearnRate.Value;
            if (request.Params.BiasReg != null)
                underlyingRecSource.BiasReg = request.Params.BiasReg.Value;
            if (request.Params.BoldDriver != null)
                underlyingRecSource.BoldDriver = request.Params.BoldDriver.Value;
            if (request.Params.FrequencyRegularization != null)
                underlyingRecSource.FrequencyRegularization = request.Params.FrequencyRegularization.Value;
            if (request.Params.LearnRate != null)
                underlyingRecSource.LearnRate = request.Params.LearnRate.Value;
            if (request.Params.NumFactors != null)
                underlyingRecSource.NumFactors = request.Params.NumFactors.Value;
            if (request.Params.NumIter != null)
                underlyingRecSource.NumIter = request.Params.NumIter.Value;
            if(request.Params.OptimizationTarget != null)
                underlyingRecSource.Loss = (OptimizationTarget)Enum.Parse(typeof(OptimizationTarget), request.Params.OptimizationTarget);
            if (request.Params.RegI != null)
                underlyingRecSource.RegI = request.Params.RegI.Value;
            if (request.Params.RegU != null)
                underlyingRecSource.RegU = request.Params.RegU.Value;

            MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization> recSource = new MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization>(
                recommender: underlyingRecSource,
                minEpisodesToCountIncomplete: request.Params.MinEpisodesToCountIncomplete,
                useDropped: request.Params.UseDropped,
                minUsersToCountAnime: request.Params.MinUsersToCountAnime
            );

            return recSource;
        }
        
        protected override MalUserListEntries GetRecSourceInputFromRequest(MalUserListEntries animeList, GetMalRecsRequest recRequest)
        {
            return animeList;
        }

        protected override void SetSpecializedRecommendationProperties(DTO.RatingPredictionRecommendation dtoRec,
            RecEngine.RatingPredictionRecommendation engineRec)
        {
            dtoRec.PredictedRating = engineRec.PredictedRating;
        }

        protected override string RecommendationType { get { return RecommendationTypes.RatingPrediction; } }
        public override string RecSourceType { get { return RecSourceTypes.BiasedMatrixFactorization; } }
    }
}

#endif
