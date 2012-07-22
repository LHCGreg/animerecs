using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecService.RecSources;
using AnimeRecs.RecEngine.MAL;
using MyMediaLite.RatingPrediction;

namespace AnimeRecs.RecService.OperationHandlers
{
    internal static partial class OpHandlers
    {
        public static Response LoadRecSource(Operation baseOperation, RecServiceState state)
        {
            Operation<LoadRecSourceRequest> operation = (Operation<LoadRecSourceRequest>)baseOperation;
            if (!operation.PayloadSet || operation.Payload == null)
                return GetArgumentNotSetError("Payload");
            if (operation.Payload.Name == null)
                return GetArgumentNotSetError("Payload.Name");
            if (operation.Payload.Type == null)
                return GetArgumentNotSetError("Payload.Type");

            ITrainableJsonRecSource recSource;

            if (operation.Payload.Type.Equals(RecSourceTypes.AverageScore, StringComparison.OrdinalIgnoreCase))
            {
                LoadRecSourceRequest<AverageScoreRecSourceParams> request = (LoadRecSourceRequest<AverageScoreRecSourceParams>)operation.Payload;
                AverageScoreRecSourceParams recSourceParams = request.Params;
                MalAverageScoreRecSource underlyingRecSource = new MalAverageScoreRecSource(
                    minEpisodesToCountIncomplete: recSourceParams.MinEpisodesToCountIncomplete,
                    useDropped: recSourceParams.UseDropped,
                    minUsersToCountAnime: recSourceParams.MinUsersToCountAnime
                );
                recSource = new AverageScoreJsonRecSource(underlyingRecSource);
            }
            else if (operation.Payload.Type.Equals(RecSourceTypes.MostPopular, StringComparison.OrdinalIgnoreCase))
            {
                LoadRecSourceRequest<MostPopularRecSourceParams> request = (LoadRecSourceRequest<MostPopularRecSourceParams>)operation.Payload;
                MostPopularRecSourceParams recSourceParams = request.Params;
                MalMostPopularRecSource underlyingRecSource = new MalMostPopularRecSource(
                    minEpisodesToCountIncomplete: recSourceParams.MinEpisodesToCountIncomplete,
                    useDropped: recSourceParams.UseDropped
                );
                recSource = new MostPopularJsonRecSource(underlyingRecSource);
            }
            else if (operation.Payload.Type.Equals(RecSourceTypes.AnimeRecs, StringComparison.OrdinalIgnoreCase))
            {
                LoadRecSourceRequest<AnimeRecsRecSourceParams> request = (LoadRecSourceRequest<AnimeRecsRecSourceParams>)operation.Payload;
                AnimeRecsRecSourceParams recSourceParams = request.Params;
                MalAnimeRecsRecSource underlyingRecSource = new MalAnimeRecsRecSource(
                    numRecommendersToUse: recSourceParams.NumRecommendersToUse,
                    fractionConsideredRecommended: recSourceParams.FractionConsideredRecommended,
                    minEpisodesToClassifyIncomplete: recSourceParams.MinEpisodesToClassifyIncomplete
                );
                recSource = new AnimeRecsJsonRecSource(underlyingRecSource);
            }
            else if (operation.Payload.Type.Equals(RecSourceTypes.BiasedMatrixFactorization, StringComparison.OrdinalIgnoreCase))
            {
                LoadRecSourceRequest<BiasedMatrixFactorizationRecSourceParams> request = (LoadRecSourceRequest<BiasedMatrixFactorizationRecSourceParams>)operation.Payload;
                BiasedMatrixFactorizationRecSourceParams recSourceParams = request.Params;
                BiasedMatrixFactorization underlyingRecSource = new BiasedMatrixFactorization();

                if (recSourceParams.BiasLearnRate != null)
                    underlyingRecSource.BiasLearnRate = recSourceParams.BiasLearnRate.Value;
                if (recSourceParams.BiasReg != null)
                    underlyingRecSource.BiasReg = recSourceParams.BiasReg.Value;
                if (recSourceParams.BoldDriver != null)
                    underlyingRecSource.BoldDriver = recSourceParams.BoldDriver.Value;
                if (recSourceParams.FrequencyRegularization != null)
                    underlyingRecSource.FrequencyRegularization = recSourceParams.FrequencyRegularization.Value;
                if (recSourceParams.LearnRate != null)
                    underlyingRecSource.LearnRate = recSourceParams.LearnRate.Value;
                if (recSourceParams.NumFactors != null)
                    underlyingRecSource.NumFactors = recSourceParams.NumFactors.Value;
                if (recSourceParams.NumIter != null)
                    underlyingRecSource.NumIter = recSourceParams.NumIter.Value;
                if (recSourceParams.OptimizationTarget != null)
                    underlyingRecSource.Loss = (OptimizationTarget)Enum.Parse(typeof(OptimizationTarget), recSourceParams.OptimizationTarget);
                if (recSourceParams.RegI != null)
                    underlyingRecSource.RegI = recSourceParams.RegI.Value;
                if (recSourceParams.RegU != null)
                    underlyingRecSource.RegU = recSourceParams.RegU.Value;

                MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization> malRecSource =
                    new MalMyMediaLiteRatingPredictionRecSource<BiasedMatrixFactorization>(underlyingRecSource,
                        recSourceParams.MinEpisodesToCountIncomplete, recSourceParams.UseDropped);

                recSource = new BiasedMatrixFactorizationJsonRecSource(malRecSource);
            }
            else
            {
                return Response.GetErrorResponse(
                    errorCode: ErrorCodes.InvalidArgument,
                    message: string.Format("{0} is not a valid rec source type.", operation.Payload.Type)
                );
            }

            state.LoadRecSource(recSource, operation.Payload.Name, operation.Payload.ReplaceExisting);

            return new Response();
        }
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