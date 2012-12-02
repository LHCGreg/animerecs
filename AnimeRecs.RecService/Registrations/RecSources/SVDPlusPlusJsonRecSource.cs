using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMediaLite.RatingPrediction;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.RecService.Registrations.RecSources
{
    [JsonRecSource(RecSourceTypes.SVDPlusPlus)]
    internal class SVDPlusPlusJsonRecSource :
        TrainableJsonRecSource<MalMyMediaLiteRatingPredictionRecSource<SVDPlusPlus>, MalUserListEntries,
        IEnumerable<RecEngine.RatingPredictionRecommendation>, RecEngine.RatingPredictionRecommendation, GetMalRecsResponse<DTO.RatingPredictionRecommendation>,
        DTO.RatingPredictionRecommendation>
    {
        public SVDPlusPlusJsonRecSource(LoadRecSourceRequest<SVDPlusPlusRecSourceParams> request)
            : base(CreateRecSourceFromRequest(request))
        {
            ;
        }

        private static MalMyMediaLiteRatingPredictionRecSource<SVDPlusPlus> CreateRecSourceFromRequest(LoadRecSourceRequest<SVDPlusPlusRecSourceParams> request)
        {
            SVDPlusPlus underlyingRecSource = new SVDPlusPlus();
            if (request.Params.BiasLearnRate != null)
                underlyingRecSource.BiasLearnRate = request.Params.BiasLearnRate.Value;
            if (request.Params.BiasReg != null)
                underlyingRecSource.BiasReg = request.Params.BiasReg.Value;
            if (request.Params.FrequencyRegularization != null)
                underlyingRecSource.FrequencyRegularization = request.Params.FrequencyRegularization.Value;
            if (request.Params.LearnRate != null)
                underlyingRecSource.LearnRate = request.Params.LearnRate.Value;
            if(request.Params.LearnRateDecay != null)
                underlyingRecSource.LearnRateDecay = request.Params.LearnRateDecay.Value;
            if (request.Params.NumFactors != null)
                underlyingRecSource.NumFactors = request.Params.NumFactors.Value;
            if (request.Params.NumIter != null)
                underlyingRecSource.NumIter = request.Params.NumIter.Value;
            if(request.Params.Regularization != null)
                underlyingRecSource.Regularization = request.Params.Regularization.Value;

            MalMyMediaLiteRatingPredictionRecSource<SVDPlusPlus> recSource = new MalMyMediaLiteRatingPredictionRecSource<SVDPlusPlus>(
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

        protected override void SetSpecializedRecommendationProperties(DTO.RatingPredictionRecommendation dtoRec, RecEngine.RatingPredictionRecommendation engineRec)
        {
            dtoRec.PredictedRating = engineRec.PredictedRating;
        }

        protected override string RecommendationType { get { return RecommendationTypes.RatingPrediction; } }
        public override string RecSourceType { get { return RecSourceTypes.SVDPlusPlus; } }
    }
}
