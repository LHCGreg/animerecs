using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.Registrations.RecSources
{
    [JsonRecSource(RecSourceTypes.AverageScore)]
    internal class AverageScoreJsonRecSource :
        TrainableJsonRecSource<MalAverageScoreRecSource, MalUserListEntries, IEnumerable<RecEngine.AverageScoreRecommendation>,
        RecEngine.AverageScoreRecommendation, GetMalRecsResponse<DTO.AverageScoreRecommendation>, DTO.AverageScoreRecommendation>
    {
        public AverageScoreJsonRecSource(LoadRecSourceRequest<AverageScoreRecSourceParams> request)
            : base(CreateRecSourceFromRequest(request))
        {
            ;
        }

        private static MalAverageScoreRecSource CreateRecSourceFromRequest(LoadRecSourceRequest<AverageScoreRecSourceParams> request)
        {
            return new MalAverageScoreRecSource(
                minEpisodesToCountIncomplete: request.Params.MinEpisodesToCountIncomplete,
                useDropped: request.Params.UseDropped,
                minUsersToCountAnime: request.Params.MinUsersToCountAnime
            );
        }
        
        protected override MalUserListEntries GetRecSourceInputFromRequest(MalUserListEntries animeList, GetMalRecsRequest recRequest)
        {
            return animeList;
        }

        protected override void SetSpecializedRecommendationProperties(DTO.AverageScoreRecommendation dtoRec, RecEngine.AverageScoreRecommendation engineRec)
        {
            dtoRec.AverageScore = engineRec.AverageScore;
            dtoRec.NumRatings = engineRec.NumRatings;
        }

        protected override string RecommendationType { get { return RecommendationTypes.AverageScore; } }
        public override string RecSourceType { get { return RecSourceTypes.AverageScore; } }
    }
}

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