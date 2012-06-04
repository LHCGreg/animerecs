﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using System.Threading;
using AnimeRecs.RecService.DTO;
using AnimeRecs.DAL;

namespace AnimeRecs.RecService
{
    /// <summary>
    /// Stores the state of the recommendation service, including rec sources. All public methods and properties are thread-safe.
    /// </summary>
    internal class RecServiceState : IDisposable
    {
        // When locking, always lock in the order the members are listed here.

        // This object is never actually modified, just repointed to another object.
        // So it is ok to get a read lock, store a reference in a variable, unlock, then train using the variable.
        private MalTrainingData m_trainingData;
        private ReaderWriterLockSlim m_trainingDataLock;
        private Dictionary<string, ITrainableJsonRecSource> m_recSources = new Dictionary<string, ITrainableJsonRecSource>(StringComparer.OrdinalIgnoreCase);
        private ReaderWriterLockSlim m_recSourcesLock;

        private IMalTrainingDataLoaderFactory m_trainingDataLoaderFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trainingDataLoaderFactory">Must be thread-safe.</param>
        public RecServiceState(IMalTrainingDataLoaderFactory trainingDataLoaderFactory)
        {
            using (IMalTrainingDataLoader trainingDataLoader = trainingDataLoaderFactory.GetTrainingDataLoader())
            {
                m_trainingData = trainingDataLoader.LoadMalTrainingData();
            }

            m_trainingDataLock = new ReaderWriterLockSlim();
            m_recSourcesLock = new ReaderWriterLockSlim();
            m_trainingDataLoaderFactory = trainingDataLoaderFactory;
        }

        public void LoadRecSource(ITrainableJsonRecSource recSource, string name, bool replaceExisting)
        {
            MalTrainingData trainingData;
            using (var trainingDataReadLock = m_trainingDataLock.ScopedReadLock())
            {
                trainingData = m_trainingData;
            }

            using (var recSourcesUpgradeableReadLock = m_recSourcesLock.ScopedUpgradeableReadLock())
            {
                if (m_recSources.ContainsKey(name) && !replaceExisting)
                {
                    throw new RecServiceErrorException(new Error(errorCode: ErrorCodes.Unknown,
                        message: string.Format("A recommendation source with the name \"{0}\" already exists.", name)));
                }

                recSource.Train(trainingData);

                using (var recSourcesWriteLock = m_recSourcesLock.ScopedWriteLock())
                {
                    m_recSources[name] = recSource;
                }
            }
        }

        public void ReloadTrainingData()
        {
            MalTrainingData newData;
            // Load new training data first
            using (IMalTrainingDataLoader malTrainingDataLoader = m_trainingDataLoaderFactory.GetTrainingDataLoader())
            {
                newData = malTrainingDataLoader.LoadMalTrainingData();
            }

            // Then swap out training data and retrain all loaded rec sources
            using (var trainingDataWriteLock = m_trainingDataLock.ScopedWriteLock())
            using (var recSourcesWriteLock = m_recSourcesLock.ScopedWriteLock())
            {
                foreach (ITrainableJsonRecSource recSource in m_recSources.Values)
                {
                    // TODO: What to do if training throws?
                    recSource.Train(newData);
                }

                m_trainingData = newData;
            }
        }

        public void Dispose()
        {
            m_trainingDataLock.Dispose();
            m_recSourcesLock.Dispose();
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