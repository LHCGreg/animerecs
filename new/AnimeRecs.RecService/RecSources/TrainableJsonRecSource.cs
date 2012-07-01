using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecEngine;
using Newtonsoft.Json;

namespace AnimeRecs.RecService.RecSources
{
    internal abstract class TrainableJsonRecSource<TMalRecSource, TInput, TRecommendationResults, TRecommendation, TResponse, TDtoRec> : ITrainableJsonRecSource
        where TMalRecSource : ITrainableRecSource<MalTrainingData, TInput, TRecommendationResults,  TRecommendation>
        where TRecommendationResults : IEnumerable<TRecommendation>
        where TInput : IInputForUser
        where TRecommendation : IRecommendation
        where TResponse : DTO.GetMalRecsResponse<TDtoRec>, new()
        where TDtoRec : DTO.Recommendation, new()
    {
        protected TMalRecSource UnderlyingRecSource { get; private set; }

        private MalTrainingData m_trainingData = new MalTrainingData();
        protected MalTrainingData TrainingData { get { return m_trainingData; } private set { m_trainingData = value; } }

        public TrainableJsonRecSource(TMalRecSource underlyingRecSource)
        {
            UnderlyingRecSource = underlyingRecSource;
        }

        public void Train(MalTrainingData trainingData)
        {
            UnderlyingRecSource.Train(trainingData);
            TrainingData = trainingData;
        }

        public DTO.GetMalRecsResponse GetRecommendations(MalUserListEntries animeList, GetMalRecsRequest recRequest, RecRequestCaster caster)
        {
            TInput recSourceInput = GetRecSourceInputFromRequest(animeList, recRequest, caster);
            TRecommendationResults recResults = UnderlyingRecSource.GetRecommendations(recSourceInput, recRequest.NumRecsDesired);

            List<TDtoRec> dtoRecs = new List<TDtoRec>();
            Dictionary<int, DTO.MalAnime> animes = new Dictionary<int, DTO.MalAnime>();
            foreach (TRecommendation rec in recResults)
            {
                TDtoRec dtoRec = new TDtoRec()
                {
                    MalAnimeId = rec.ItemId,
                    //Title = TrainingData.Animes[rec.ItemId].Title,
                    //MalAnimeType = TrainingData.Animes[rec.ItemId].Type
                };
                animes[rec.ItemId] = new DTO.MalAnime(rec.ItemId, TrainingData.Animes[rec.ItemId].Title, TrainingData.Animes[rec.ItemId].Type);

                SetSpecializedRecommendationProperties(dtoRec, rec);

                dtoRecs.Add(dtoRec);
            }

            //GetMalRecsResponse<TDtoRec> response = CreateResponseDto(dtoRecs);
            TResponse response = new TResponse();
            response.RecommendationType = RecommendationType;
            response.Recommendations = dtoRecs;
            SetSpecializedExtraResponseProperties(response, recResults);

            HashSet<int> extraAnimesToReturn = GetExtraAnimesToReturn(recResults);
            foreach (int extraAnimeId in extraAnimesToReturn)
            {
                if (!animes.ContainsKey(extraAnimeId))
                {
                    animes[extraAnimeId] = new DTO.MalAnime(extraAnimeId, TrainingData.Animes[extraAnimeId].Title, TrainingData.Animes[extraAnimeId].Type);
                }
            }

            response.Animes = animes.Values.ToList();

            return response;
        }

        protected abstract TInput GetRecSourceInputFromRequest(MalUserListEntries animeList, GetMalRecsRequest recRequest, RecRequestCaster caster);
        protected abstract void SetSpecializedRecommendationProperties(TDtoRec dtoRec, TRecommendation engineRec);
        protected abstract string RecommendationType { get; }

        protected virtual void SetSpecializedExtraResponseProperties(TResponse response, TRecommendationResults recResults)
        {
            ;
        }

        protected virtual HashSet<int> GetExtraAnimesToReturn(TRecommendationResults recResults)
        {
            return new HashSet<int>();
        }

        public override string ToString()
        {
            return UnderlyingRecSource.ToString();
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