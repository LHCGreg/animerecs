#if MYMEDIALITE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyMediaLite.RatingPrediction;
using MyMediaLite.Correlation;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecEngine;
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

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.RecService.
//
// AnimeRecs.RecService is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.  If not, see <http://www.gnu.org/licenses/>.
