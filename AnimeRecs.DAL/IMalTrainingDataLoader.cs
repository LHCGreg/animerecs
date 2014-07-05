using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecEngine.MAL;

namespace AnimeRecs.DAL
{
    public interface IMalTrainingDataLoader : IDisposable
    {
        MalTrainingData LoadMalTrainingData();

        /// <summary>
        /// Returns a dictionary where the key is the MAL anime id of an anime and the value is a list of MAL anime ids of prerequisites
        /// for that anime. If a key is not in the dictionary, that anime has no known prerequisites.
        /// </summary>
        /// <returns></returns>
        IDictionary<int, IList<int>> LoadPrerequisites();
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.DAL.
//
// AnimeRecs.DAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.DAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.DAL.  If not, see <http://www.gnu.org/licenses/>.