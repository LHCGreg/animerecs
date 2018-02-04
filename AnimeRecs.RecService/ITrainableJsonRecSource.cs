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
