﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.DAL;
using System.Text.RegularExpressions;

namespace AnimeRecs.UpdateStreams
{
    class FunimationStreamInfoSource : HtmlRegexAnimeStreamInfoSource
    {
        // <li><a class="fs16 bold" href="http://www.funimation.com/shows/hackquantum/videos/episodes/anime">.hack//Quantum</a></li>

        public FunimationStreamInfoSource()
            : base(url: "http://www.funimation.com/videos", service: StreamingService.Funimation,
            animeNameContext: HtmlRegexContext.Body, urlContext: HtmlRegexContext.Attribute,
            animeRegex: new Regex("<li><a class=\"fs16 bold\" href=\"(?<Url>[^\"]*)\">(?<AnimeName>.*?)</a>",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline))
        {
            ;
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.UpdateStreams
//
// AnimeRecs.UpdateStreams is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.UpdateStreams is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.UpdateStreams.  If not, see <http://www.gnu.org/licenses/>.
//
//  If you modify AnimeRecs.UpdateStreams, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.