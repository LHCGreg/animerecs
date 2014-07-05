using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecService.DTO
{
    public static class OpNames
    {
        public static string Ping { get { return "Ping"; } }
        public static string LoadRecSource { get { return "LoadRecSource"; } }
        public static string UnloadRecSource { get { return "UnloadRecSource"; } }
        public static string ReloadTrainingData { get { return "ReloadTrainingData"; } }
        public static string GetMalRecs { get { return "GetMalRecs"; } }
        public static string GetRecSourceType { get { return "GetRecSourceType"; } }
        public static string FinalizeRecSources { get { return "FinalizeRecSources"; } }
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