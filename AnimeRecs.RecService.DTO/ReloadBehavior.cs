using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public enum ReloadBehavior
    {
        /// <summary>
        /// Old loaded data is discarded before reloading from the database and retraining.
        /// This avoids having to double memory usage but blocks all other operations for the duration of the reload and retrain.
        /// If an error occurs reloading the data from the database, no rec sources will be loaded.
        /// If an error occurs retraining a rec source, that rec source will be dropped.
        /// </summary>
        LowMemory,

        /// <summary>
        /// Keep old rec sources around during a reload/retrain.
        /// This requires double the memory of normal use but keeps the rec service responsive while the reload/retrain is going on.
        /// If an error occurs reloading the data from the database, all rec sources remain loaded.
        /// If an error occurs retraining a rec source, that rec source will be dropped.
        /// </summary>
        HighAvailability
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecService.DTO.
//
// AnimeRecs.RecService.DTO is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.DTO is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.DTO.  If not, see <http://www.gnu.org/licenses/>.