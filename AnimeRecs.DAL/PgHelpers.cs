using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.DAL
{
    internal static class PgHelpers
    {
        /// <summary>
        /// Creates a string literal suitable for use with PostgreSQL by placing the string in "double quotes" and replacing
        /// any quote characters (') with two quote characters ('').
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string QuotePgString(string str)
        {
            return "'" + str.Replace("'", "''") + "'";
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.DAL.
//
// AnimeRecs.DAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.DAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.DAL.  If not, see <http://www.gnu.org/licenses/>.