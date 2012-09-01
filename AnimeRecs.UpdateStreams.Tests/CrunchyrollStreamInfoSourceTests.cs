using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AnimeRecs.UpdateStreams;
using AnimeRecs.DAL;

namespace AnimeRecs.UpdateStreams.Tests
{
    [TestFixture]
    public partial class CrunchyrollStreamInfoSourceTests
    {
        [Test]
        public void TestRegex()
        {
            CrunchyrollStreamInfoSource cr = new CrunchyrollStreamInfoSource();
            ICollection<AnimeStreamInfo> streams = cr.GetAnimeStreamInfo(CrunchyrollStreamInfoSourceTests.TestHtml);
            Assert.That(streams, Contains.Item(new AnimeStreamInfo("Mobile Suit Zeta Gundam", "http://www.crunchyroll.com/mobile-suit-zeta-gundam", StreamingService.Crunchyroll)));
            Assert.That(streams, Contains.Item(new AnimeStreamInfo("NARUTO Spin-Off: Rock Lee & His Ninja Pals", "http://www.crunchyroll.com/naruto-spin-off-rock-lee-his-ninja-pals", StreamingService.Crunchyroll)));
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.UpdateStreams.Tests
//
// AnimeRecs.UpdateStreams.Tests is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.UpdateStreams.Tests is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.UpdateStreams.Tests.  If not, see <http://www.gnu.org/licenses/>.