using AnimeRecs.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams
{
    class AmazonAnimeStrikeStreamInfoSource : AmazonStreamInfoSource
    {
        private const string FirstPageUrl = "https://www.amazon.com/s/ref=sr_nr_p_n_subscription_id_1?rh=n:2858778011,p_n_ways_to_watch:12007866011,p_n_subscription_id:16182082011&ie=UTF8";

        public AmazonAnimeStrikeStreamInfoSource(IWebClient webClient)
            : base(FirstPageUrl, StreamingService.AmazonAnimeStrike, webClient)
        {

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
