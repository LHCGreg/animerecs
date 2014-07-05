﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.RecService.DTO;

namespace AnimeRecs.RecService.OperationHandlers
{
    internal static partial class OpHandlers
    {
        public static Response ReloadTrainingData(Operation baseOperation, RecServiceState state)
        {
            Operation<ReloadTrainingDataRequest> operation = (Operation<ReloadTrainingDataRequest>)baseOperation;
            ReloadBehavior behavior = (ReloadBehavior)Enum.Parse(typeof(ReloadBehavior), operation.Payload.Mode);
            state.ReloadTrainingData(behavior, operation.Payload.Finalize);
            return new Response();
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