using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnimeRecs.RecService.ClientLib;

namespace AnimeRecs.NancyWeb
{
    public class RecClientFactory : IAnimeRecsClientFactory
    {
        private int? DefaultPort { get; set; }
        private IDictionary<string, int> SpecialRecSourcePorts { get; set; }

        public RecClientFactory(int? defaultPort, IDictionary<string, int> specialRecSourcePorts)
        {
            DefaultPort = defaultPort;
            SpecialRecSourcePorts = specialRecSourcePorts;
        }

        public AnimeRecsClient GetClient(string recSourceName)
        {
            if (recSourceName != null && SpecialRecSourcePorts.ContainsKey(recSourceName))
            {
                int port = SpecialRecSourcePorts[recSourceName];
                return new AnimeRecsClient(port);
            }
            else
            {
                if (DefaultPort == null)
                {
                    return new AnimeRecsClient();
                }
                else
                {
                    return new AnimeRecsClient(DefaultPort.Value);
                }
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