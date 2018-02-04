using System;
using System.Collections.Generic;
using System.Linq;
using AnimeRecs.DAL;

namespace AnimeRecs.RecService
{
    internal class PgMalTrainingDataLoaderFactory : IMalTrainingDataLoaderFactory
    {
        private string m_connectionString;

        public PgMalTrainingDataLoaderFactory(string connectionString)
        {
            m_connectionString = connectionString;
        }
        
        public IMalTrainingDataLoader GetTrainingDataLoader()
        {
            return new PgMalDataLoader(m_connectionString);
        }
    }
}
