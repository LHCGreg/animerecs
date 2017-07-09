using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace AnimeRecs.UpdateStreams
{
    static class Utils
    {
        public static string PossiblyRelativeUrlToAbsoluteUrl(string possiblyRelativeUrl, string sourceUrl)
        {
            Uri possiblyRelativeUri = new Uri(possiblyRelativeUrl, UriKind.RelativeOrAbsolute);
            if (possiblyRelativeUri.IsAbsoluteUri)
            {
                return possiblyRelativeUri.ToString();
            }
            else
            {
                return new Uri(new Uri(sourceUrl), possiblyRelativeUrl).ToString();
            }
        }

        public static string DecodeHtmlAttribute(string rawAttributeText)
        {
            return WebUtility.HtmlDecode(rawAttributeText);
        }

        public static string DecodeHtmlBody(string rawBody)
        {
            return WebUtility.HtmlDecode(rawBody);
        }
    }
}

// Copyright (C) 2017 Greg Najda
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
