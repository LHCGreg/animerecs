using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService
{
    internal interface ITrainableJsonRecSource
    {
        // Pass in usernames even though it could be derived from trainingData so that the same reference can be shared across rec sources
        // If training can be length, the rec source is expected to periodically check the cancellation token
        void Train(MalTrainingData trainingData, IDictionary<int, string> usernamesByUserId, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recRequest"></param>
        /// <returns></returns>
        /// <exception cref="AnimeRecs.RecService.DTO.RecServiceErrorException">Throw to return a rec service error to the client.</exception>
        GetMalRecsResponse GetRecommendations(MalUserListEntries animeList, GetMalRecsRequest recRequest, CancellationToken cancellationToken);

        string RecSourceType { get; }
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