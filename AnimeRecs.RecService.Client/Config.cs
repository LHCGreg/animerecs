using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace AnimeRecs.RecService.Client
{
    internal class Config
    {
        public ApiType MalApiType { get; set; } = ApiType.Normal;
        public ConfigConnectionStrings ConnectionStrings { get; set; }

        internal static Config LoadFromFile(string filePath)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .AddXmlFile(filePath);

            IConfigurationRoot rawConfig = configBuilder.Build();
            return rawConfig.Get<Config>();
        }

        public class ConfigConnectionStrings
        {
            public string AnimeRecs { get; set; }
        }

        public enum ApiType
        {
            Normal,
            DB
        }
    }
}

// Copyright (C) 2017 Greg Najda
//
// This file is part of AnimeRecs.RecService.Client.
//
// AnimeRecs.RecService.Client is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecService.Client is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecService.Client.  If not, see <http://www.gnu.org/licenses/>.