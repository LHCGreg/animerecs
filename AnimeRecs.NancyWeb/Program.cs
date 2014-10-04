using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Hosting.Self;

namespace AnimeRecs.NancyWeb
{
    class Program
    {
        static void Main(string[] args)
        {
            HostConfiguration config = new HostConfiguration()
            {
                RewriteLocalhost = false
            };

            using (var host = new NancyHost(config, new Uri("http://localhost:8888")))
            {
                host.Start();
                Console.WriteLine("Started");
                Console.ReadLine();
            }
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