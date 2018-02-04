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
