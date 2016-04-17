﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using HtmlAgilityPack;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams
{
    class VizStreamInfoSource : HtmlParsingAnimeStreamInfoSource
    {
        private const string Url = "http://www.viz.com/watch/streaming/watch";
        private const string XPath = @"//table[@class=""purchase-table""]/tbody/tr/td/a";
        
        public VizStreamInfoSource()
            : base(Url, XPath)
        {
            ;
        }

        protected override AnimeStreamInfo GetStreamInfoFromMatch(HtmlNode matchingNode)
        {
            string possiblyRelativeUrl = matchingNode.Attributes["href"].Value;
            string animeName = HtmlEntity.DeEntitize(matchingNode.InnerText);
            return new AnimeStreamInfo(animeName, possiblyRelativeUrl, StreamingService.Viz);
        }
    }
}

// Copyright (C) 2016 Greg Najda
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