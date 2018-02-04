using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.DAL
{
    public interface IMalTrainingDataLoader : IDisposable
    {
        Task<MalTrainingData> LoadMalTrainingDataAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Returns a dictionary where the key is the MAL anime id of an anime and the value is a list of MAL anime ids of prerequisites
        /// for that anime. If a key is not in the dictionary, that anime has no known prerequisites.
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<int, IList<int>>> LoadPrerequisitesAsync(CancellationToken cancellationToken);
    }
}
