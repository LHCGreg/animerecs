using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnimeRecs.RecEngine;
using AnimeRecs.RecEngine.MAL;
using AnimeRecs.RecService.ClientLib;

namespace AnimeRecs.Web.Models.ViewModels
{
    public class GetRecsViewModel
    {
        public MalRecResults<IEnumerable<IRecommendation>> Results { get; private set; }
        public int UserId { get; private set; }
        public string UserName { get; private set; }
        public IDictionary<int, MalListEntry> UserAnimeList { get; private set; }

        public GetRecsViewModel(MalRecResults<IEnumerable<IRecommendation>> results, int userId, string userName, IDictionary<int, MalListEntry> userAnimeList)
        {
            Results = results;
            UserId = userId;
            UserName = userName;
            UserAnimeList = userAnimeList;
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.Web.
//
// AnimeRecs.Web is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.Web is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.Web.  If not, see <http://www.gnu.org/licenses/>.