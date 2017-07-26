using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeRecs.Utils
{
    public struct CancellableAsyncFunc
    {
        public Func<Task> Function { get; private set; }
        public CancellationTokenSource Cancel { get; private set; }

        public CancellableAsyncFunc(Func<Task> function, CancellationTokenSource cancel)
        {
            Function = function;
            Cancel = cancel;
        }

        public CancellableTask StartTaskEnsureExceptionsWrapped()
        {
            Task task = AsyncUtils.EnsureExceptionsWrapped(Function);
            return new CancellableTask(task, Cancel);
        }
    }

    public struct CancellableAsyncFunc<T>
    {
        public Func<Task<T>> Function { get; private set; }
        public CancellationTokenSource Cancel { get; private set; }

        public CancellableAsyncFunc(Func<Task<T>> function, CancellationTokenSource cancel)
        {
            Function = function;
            Cancel = cancel;
        }

        public CancellableTask<T> StartTaskEnsureExceptionsWrapped()
        {
            Task<T> task = AsyncUtils.EnsureExceptionsWrapped(Function);
            return new CancellableTask<T>(task, Cancel);
        }
    }
}

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.Utils
//
// AnimeRecs.Utils is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.Utils is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.Utils.  If not, see <http://www.gnu.org/licenses/>.
