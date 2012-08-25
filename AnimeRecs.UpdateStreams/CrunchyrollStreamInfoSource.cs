using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AnimeRecs.UpdateStreams
{
    class CrunchyrollStreamInfoSource : HttpRegexAnimeStreamInfoSource
    {
        private const string AnimeListUrl = "http://www.crunchyroll.com/videos/anime/alpha?group=all";
        
        public CrunchyrollStreamInfoSource()
            : base(url: AnimeListUrl, service: AnimeRecs.DAL.StreamingService.Crunchyroll,
            animeNameContext: HttpRegexContext.Body, urlContext: HttpRegexContext.Attribute,
            animeRegex: new Regex("<a title=\"[^\"]*?\" token=\"shows-portraits\" itemprop=\"url\" href=\"(?<Url>[^\"]*?)\" [^>]*?>\\s*(?<AnimeName>.*?)\\s*?</a>", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline))
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