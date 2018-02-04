using System;
using System.Collections.Generic;
using System.Linq;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.DTO;
using AnimeRecs.RecEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;

namespace AnimeRecs.RecService.Registrations.RecSources
{
    internal abstract class TrainableJsonRecSource<TMalRecSource, TInput, TRecommendationResults, TRecommendation, TResponse, TDtoRec>
        : ITrainableJsonRecSource

        where TMalRecSource : ITrainableRecSource<MalTrainingData, TInput, TRecommendationResults, TRecommendation>
        where TRecommendationResults : IEnumerable<TRecommendation>
        where TInput : IInputForUser
        where TRecommendation : IRecommendation
        where TResponse : DTO.GetMalRecsResponse<TDtoRec>, new()
        where TDtoRec : DTO.Recommendation, new()
    {
        protected TMalRecSource UnderlyingRecSource { get; private set; }
        private object m_underlyingRecSourceLock = new object();

        private IDictionary<int, string> m_usernamesByUserId = new Dictionary<int, string>();
        protected IDictionary<int, string> UsernamesByUserId { get { return m_usernamesByUserId; } private set { m_usernamesByUserId = value; } }

        private IDictionary<int, RecEngine.MAL.MalAnime> m_animes = new Dictionary<int, RecEngine.MAL.MalAnime>();
        protected IDictionary<int, RecEngine.MAL.MalAnime> Animes { get { return m_animes; } private set { m_animes = value; } }

        public TrainableJsonRecSource(TMalRecSource underlyingRecSource)
        {
            UnderlyingRecSource = underlyingRecSource;
        }

        // Cancellation token is mainly so we can stop the service in a timely manner even if a lengthy train is going on.
        // Adding cooperative cancellation to the underlying rec sources be some work.
        // So instead, this base class implements "uncooperative" cancellation.
        // Run the training on another thread and kill the thread on cancellation.
        // Killing it will leave the rec source in some indeterminate state, but that's fine if we're shutting down anyway.
        // That was the idea, anyway, but .NET core does not support Thread.Abort() until the yet-to-be-released 2.0.
        // So instead, just abandon the thread to churn along for the brief period until the process exits.
        public void Train(MalTrainingData trainingData, IDictionary<int, string> usernamesByUserId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Task trainingTask = Task.Factory.StartNew(() => TrainingThreadEntryPoint(trainingData), TaskCreationOptions.LongRunning);
            try
            {
                trainingTask.Wait(cancellationToken);
            }
            catch (AggregateException aggEx)
            {
                ExceptionDispatchInfo.Capture(aggEx.InnerException).Throw();
                throw; // Will never be reached
            }

            // Force a memory fence after off-thread changes to UnderlyingRecSource
            lock (m_underlyingRecSourceLock)
            {
                Animes = trainingData.Animes;
                UsernamesByUserId = usernamesByUserId;
            }
        }

        private void TrainingThreadEntryPoint(MalTrainingData trainingData)
        {
            lock (m_underlyingRecSourceLock)
            {
                UnderlyingRecSource.Train(trainingData);
            }
        }

        public DTO.GetMalRecsResponse GetRecommendations(MalUserListEntries animeList, GetMalRecsRequest recRequest, CancellationToken cancellationToken)
        {
            // Ignore the cancellation token - we're only cancelling on service shut down, and getting recommendations should be quick.
            // If getting recommendations takes longer than the final connection drain time limit, something is wrong.
            TInput recSourceInput = GetRecSourceInputFromRequest(animeList, recRequest);
            TRecommendationResults recResults = UnderlyingRecSource.GetRecommendations(recSourceInput, recRequest.NumRecsDesired);

            List<TDtoRec> dtoRecs = new List<TDtoRec>();
            Dictionary<int, DTO.MalAnime> animes = new Dictionary<int, DTO.MalAnime>();
            foreach (TRecommendation rec in recResults)
            {
                TDtoRec dtoRec = new TDtoRec()
                {
                    MalAnimeId = rec.ItemId,
                };
                animes[rec.ItemId] = new DTO.MalAnime(rec.ItemId, Animes[rec.ItemId].Title, Animes[rec.ItemId].Type);

                SetSpecializedRecommendationProperties(dtoRec, rec);

                dtoRecs.Add(dtoRec);
            }

            TResponse response = new TResponse();
            response.RecommendationType = RecommendationType;
            response.Recommendations = dtoRecs;
            SetSpecializedExtraResponseProperties(response, recResults);

            HashSet<int> extraAnimesToReturn = GetExtraAnimesToReturn(recResults);
            foreach (int extraAnimeId in extraAnimesToReturn)
            {
                if (!animes.ContainsKey(extraAnimeId))
                {
                    animes[extraAnimeId] = new DTO.MalAnime(extraAnimeId, Animes[extraAnimeId].Title, Animes[extraAnimeId].Type);
                }
            }

            response.Animes = animes.Values.ToList();

            return response;
        }

        /// <summary>
        /// Converts the user's anime list into the input used by the rec source.
        /// </summary>
        /// <param name="animeList"></param>
        /// <param name="recRequest"></param>
        /// <param name="caster"></param>
        /// <returns></returns>
        protected abstract TInput GetRecSourceInputFromRequest(MalUserListEntries animeList, GetMalRecsRequest recRequest);

        /// <summary>
        /// Set any properties of a recommendation other than the item id here.
        /// </summary>
        /// <param name="dtoRec"></param>
        /// <param name="engineRec"></param>
        protected abstract void SetSpecializedRecommendationProperties(TDtoRec dtoRec, TRecommendation engineRec);

        protected abstract string RecommendationType { get; }
        public abstract string RecSourceType { get; }

        /// <summary>
        /// Set any properties of the response as a whole here. That is, properties that are not per-recommendation but apply
        /// to the entire response. The default implementation does nothing.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="recResults"></param>
        protected virtual void SetSpecializedExtraResponseProperties(TResponse response, TRecommendationResults recResults)
        {
            ;
        }

        /// <summary>
        /// Returns a set of item ids that are referenced in the specialized extra response properties. The default implementation
        /// returns an empty set. If you refer to anime ids in the specialized extra response properties, you must include them here.
        /// </summary>
        /// <param name="recResults"></param>
        /// <returns></returns>
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
