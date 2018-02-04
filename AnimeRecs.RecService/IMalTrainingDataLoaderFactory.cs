using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.DAL;

namespace AnimeRecs.RecService
{
    internal interface IMalTrainingDataLoaderFactory
    {
        IMalTrainingDataLoader GetTrainingDataLoader();
    }
}
