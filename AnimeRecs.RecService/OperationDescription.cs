using System;
using System.Collections.Generic;
using System.Linq;

namespace AnimeRecs.RecService
{
    internal class OperationDescription
    {
        public Type OperationType { get; private set; }
        
        // Right now this is only used as documentation.
        public Type ResponseType { get; private set; }

        public OperationHandler OperationHandler { get; private set; }

        public OperationDescription(OperationHandler operationHandler, Type operationType, Type responseType)
        {
            OperationHandler = operationHandler;
            OperationType = operationType;
            ResponseType = responseType;
        }
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