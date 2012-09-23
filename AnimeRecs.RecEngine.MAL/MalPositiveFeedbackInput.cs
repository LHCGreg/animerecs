using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    public class MalPositiveFeedbackInput : IInputForUser
    {
        public MalUserListEntries AnimeList { get; private set; }
        public double? TargetFraction { get; private set; }
        public decimal? TargetScore { get; private set; }

        public MalPositiveFeedbackInput(MalUserListEntries animeList, double targetFraction)
        {
            AnimeList = animeList;
            TargetFraction = targetFraction;
        }

        public MalPositiveFeedbackInput(MalUserListEntries animeList, decimal targetScore)
        {
            AnimeList = animeList;
            TargetScore = targetScore;
        }

        public bool ItemIsOkToRecommend(int itemId)
        {
            return AnimeList.ItemIsOkToRecommend(itemId);
        }

        public bool ContainsItem(int itemId)
        {
            return AnimeList.ContainsItem(itemId);
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.MAL.
//
// AnimeRecs.RecEngine.MAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine.MAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.MAL.  If not, see <http://www.gnu.org/licenses/>.