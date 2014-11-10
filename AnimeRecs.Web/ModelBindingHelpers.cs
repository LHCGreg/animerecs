using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Validation;

namespace AnimeRecs.Web
{
    static class ModelBindingHelpers
    {
        public static string ConstructErrorString(IDictionary<string, IList<ModelValidationError>> errors)
        {
            List<string> errorList = new List<string>();
            foreach (var x in errors.SelectMany(p => p.Value.Select(e => new { Property = p.Key, Properties = e.MemberNames.ToList(), Error = e.ErrorMessage })))
            {
               errorList.Add(string.Format("Error with property {0}: {1}", x.Property, x.Error));
            }
            string errorString = string.Join("\n\n", errorList);
            return errorString;
        }
    }
}

// Copyright (C) 2014 Greg Najda
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