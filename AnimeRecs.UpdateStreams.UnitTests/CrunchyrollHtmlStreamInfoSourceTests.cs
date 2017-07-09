using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimeRecs.UpdateStreams;
using AnimeRecs.DAL;
using AnimeRecs.UpdateStreams.Crunchyroll;
using Xunit;

namespace AnimeRecs.UpdateStreams.UnitTests
{
    public class CrunchyrollHtmlStreamInfoSourceTests
    {
        [Fact]
        public void TestRegex()
        {
            CrunchyrollHtmlStreamInfoSource cr = new CrunchyrollHtmlStreamInfoSource(Helpers.GetResourceText("crunchyroll.html"));
            ICollection<AnimeStreamInfo> streams = cr.GetAnimeStreamInfo();

            Assert.Contains(new AnimeStreamInfo("Mobile Suit Zeta Gundam", "http://www.crunchyroll.com/mobile-suit-zeta-gundam", StreamingService.Crunchyroll), streams);
            Assert.Contains(new AnimeStreamInfo("NARUTO Spin-Off: Rock Lee & His Ninja Pals", "http://www.crunchyroll.com/naruto-spin-off-rock-lee-his-ninja-pals", StreamingService.Crunchyroll), streams);
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
