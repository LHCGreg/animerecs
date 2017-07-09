using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AnimeRecs.UpdateStreams.UnitTests
{
    static class Helpers
    {
        private static string GetResourceName(string fileName)
        {
            return "AnimeRecs.UpdateStreams.UnitTests." + fileName;
        }

        public static StreamReader GetResourceStream(string fileName)
        {
            string resourceName = GetResourceName(fileName);
            return new StreamReader(typeof(Helpers).GetTypeInfo().Assembly.GetManifestResourceStream(resourceName), Encoding.UTF8);
        }

        public static string GetResourceText(string fileName)
        {
            using (StreamReader resourceStream = GetResourceStream(fileName))
            {
                return resourceStream.ReadToEnd();
            }
        }
    }
}

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.UpdateStreams.UnitTests
//
// AnimeRecs.UpdateStreams.UnitTests is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.UpdateStreams.UnitTests is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.UpdateStreams.UnitTests.  If not, see <http://www.gnu.org/licenses/>.
