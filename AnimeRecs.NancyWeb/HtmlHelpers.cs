using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.ViewEngines.Razor;

namespace AnimeRecs.NancyWeb
{
    static class HtmlHelpers
    {
        public static IHtmlString Attribute<TModel>(string text)
        {
            // Don't use System.Web.HttpUtility, we're avoiding System.Web
            // " & <
            StringBuilder encodedString = new StringBuilder(text.Length + 2);
            encodedString.Append("\"");
            foreach (char c in text)
            {
                switch (c)
                {
                    case '"':
                        encodedString.Append("&quot;");
                        break;
                    case '&':
                        encodedString.Append("&amp;");
                        break;
                    case '<':
                        encodedString.Append("&lt;");
                        break;
                    default:
                        encodedString.Append(c);
                        break;
                }
            }
            encodedString.Append("\"");
            return new EncodedHtmlString(encodedString.ToString());
        }
    }
}

// Copyright (C) 2014 Greg Najda
//
// This file is part of AnimeRecs.NancyWeb.
//
// AnimeRecs.NancyWeb is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.NancyWeb is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.NancyWeb.  If not, see <http://www.gnu.org/licenses/>.