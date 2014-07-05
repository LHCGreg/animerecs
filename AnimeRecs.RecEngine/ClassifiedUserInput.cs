using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine
{
    public class ClassifiedUserInput<TInput>
    {
        public ClassifiedUserInput(TInput liked, TInput notLiked, TInput other)
        {
            Liked = liked;
            NotLiked = notLiked;
            Other = other;
        }

        public TInput Liked { get; private set; }
        public TInput NotLiked { get; private set; }

        /// <summary>
        /// "Normal" input should be either Liked or NotLiked. But some input types may have additional entries, such as "plan to watch",
        /// which do not belong in either. Such entries should be put here. Liked + NotLiked + Other should be a complete picture of the
        /// input.
        /// </summary>
        public TInput Other { get; private set; }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.
//
// AnimeRecs.RecEngine is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.  If not, see <http://www.gnu.org/licenses/>.