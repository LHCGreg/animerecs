#if MYMEDIALITE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMediaLite.RatingPrediction;
using MyMediaLite.Correlation;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.RecService.Registrations.RecSources
{
    [JsonRecSource(RecSourceTypes.ItemKNN)]
    internal class ItemKNNJsonRecSource :
        TrainableJsonRecSource<MalMyMediaLiteRatingPredictionRecSource<ItemKNN>, MalUserListEntries,
        IEnumerable<RecEngine.RatingPredictionRecommendation>, RecEngine.RatingPredictionRecommendation, GetMalRecsResponse<DTO.RatingPredictionRecommendation>,
        DTO.RatingPredictionRecommendation>
    {
        public ItemKNNJsonRecSource(LoadRecSourceRequest<ItemKNNRecSourceParams> request)
            : base(CreateRecSourceFromRequest(request))
        {
            ;
        }

        private static MalMyMediaLiteRatingPredictionRecSource<ItemKNN> CreateRecSourceFromRequest(LoadRecSourceRequest<ItemKNNRecSourceParams> request)
        {
            ItemKNN underlyingRecSource = new ItemKNN();
            if (request.Params.Alpha != null)
                underlyingRecSource.Alpha = request.Params.Alpha.Value;
            if (request.Params.Correlation != null)
                underlyingRecSource.Correlation = (RatingCorrelationType)Enum.Parse(typeof(RatingCorrelationType), request.Params.Correlation);
            if (request.Params.K != null)
                underlyingRecSource.K = request.Params.K.Value;
            if (request.Params.NumIter != null)
                underlyingRecSource.NumIter = request.Params.NumIter.Value;
            if (request.Params.RegI != null)
                underlyingRecSource.RegI = request.Params.RegI.Value;
            if (request.Params.RegU != null)
                underlyingRecSource.RegU = request.Params.RegU.Value;
            if (request.Params.WeightedBinary != null)
                underlyingRecSource.WeightedBinary = request.Params.WeightedBinary.Value;

            MalMyMediaLiteRatingPredictionRecSource<ItemKNN> recSource = new MalMyMediaLiteRatingPredictionRecSource<ItemKNN>(
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
        public override string RecSourceType { get { return RecSourceTypes.ItemKNN; } }
    }
}

#endif

