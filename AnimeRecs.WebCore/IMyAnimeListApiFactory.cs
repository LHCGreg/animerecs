using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MalApi;

namespace AnimeRecs.WebCore
{
    public interface IMyAnimeListApiFactory
    {
        IMyAnimeListApi GetMalApi();
    }
}

// Copyright (C) 2018 Greg Najda
//
// This file is part of AnimeRecs.WebCore.
//
// AnimeRecs.WebCore is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.WebCore is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.WebCore.  If not, see <http://www.gnu.org/licenses/>.