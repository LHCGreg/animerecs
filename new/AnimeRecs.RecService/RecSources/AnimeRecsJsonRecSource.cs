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