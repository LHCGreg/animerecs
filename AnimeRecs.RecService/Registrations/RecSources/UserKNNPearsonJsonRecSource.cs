using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using MyMediaLite.RatingPrediction;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.Registrations.RecSources
{
    [JsonRecSource(RecSourceTypes.UserKNNPearson)]
    internal class UserKNNPearsonJsonRecSource :
        TrainableJsonRecSource<MalMyMediaLiteRatingPredictionRecSource<UserKNNPearson>, MalUserListEntries,
        IEnumerable<RecEngine.RatingPredictionRecommendation>, RecEngine.RatingPredictionRecommendation, GetMalRecsResponse<DTO.RatingPredictionRecommendation>,
        DTO.RatingPredictionRecommendation>
    {
        public UserKNNPearsonJsonRecSource(LoadRecSourceRequest<UserKNNPearsonRecSourceParams> request)
            : base(CreateRecSourceFromRequest(request))
        {
            ;
        }

        private static MalMyMediaLiteRatingPredictionRecSource<UserKNNPearson> CreateRecSourceFromRequest(LoadRecSourceRequest<UserKNNPearsonRecSourceParams> request)
        {
            UserKNNPearson underlyingRecSource = new UserKNNPearson();
            if (request.Params.K != null)
                underlyingRecSource.K = request.Params.K.Value;
            if (request.Params.NumIter != null)
                underlyingRecSource.NumIter = request.Params.NumIter.Value;
            if (request.Params.RegI != null)
                underlyingRecSource.RegI = request.Params.RegI.Value;
            if (request.Params.RegU != null)
                underlyingRecSource.RegU = request.Params.RegU.Value;
            if (request.Params.Shrinkage != null)
                underlyingRecSource.Shrinkage = request.Params.Shrinkage.Value;

            return new MalMyMediaLiteRatingPredictionRecSource<UserKNNPearson>(
                recommender: underlyingRecSource,
                minEpisodesToCountIncomplete: request.Params.MinEpisodesToCountIncomplete,
                useDropped: request.Params.UseDropped,
                minUsersToCountAnime: request.Params.MinUsersToCountAnime
            );
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
        public override string RecSourceType { get { return RecSourceTypes.UserKNNPearson; } }
    }
}
