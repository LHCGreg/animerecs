using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using MyMediaLite.RatingPrediction;
using AnimeRecs.RecEngine;
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
                useDropped: request.Params.UseDropped
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

// Copyright (C) 2012 Greg Najda
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