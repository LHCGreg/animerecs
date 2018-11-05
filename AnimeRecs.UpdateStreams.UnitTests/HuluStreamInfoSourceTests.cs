using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using FluentAssertions;
using Moq;
using Xunit;

namespace AnimeRecs.UpdateStreams.UnitTests
{
    public class HuluStreamInfoSourceTests
    {
        [Fact]
        public void TestHelperStreamInfoSource()
        {
            IWebClient mockWebClient = GetMockWebClient();
            HuluStreamInfoSource.HuluStartPageStreamInfoSource source = new HuluStreamInfoSource.HuluStartPageStreamInfoSource(1, mockWebClient);
            ICollection<AnimeStreamInfo> streams = source.GetAnimeStreamInfoAsync(CancellationToken.None).GetAwaiter().GetResult();
            streams.Should().BeEquivalentTo(ExpectedFirstPageStreamInfo);
        }

        [Fact]
        public void TestHelperStreamInfoSourceOnEmpty()
        {
            IWebClient mockWebClient = GetMockWebClient();
            HuluStreamInfoSource.HuluStartPageStreamInfoSource source = new HuluStreamInfoSource.HuluStartPageStreamInfoSource(3, mockWebClient);
            Assert.Throws<NoMatchingHtmlException>(() => source.GetAnimeStreamInfoAsync(CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void TestGetAnimeStreamInfo()
        {
            IWebClient mockWebClient = GetMockWebClient();
            HuluStreamInfoSource source = new HuluStreamInfoSource(mockWebClient);
            ICollection<AnimeStreamInfo> streams = source.GetAnimeStreamInfoAsync(CancellationToken.None).GetAwaiter().GetResult();
            streams.Should().BeEquivalentTo(ExpectedStreamInfo);
        }

        private static IWebClient GetMockWebClient()
        {
            var webClientMock = new Mock<IWebClient>();

            Dictionary<Uri, IWebClientResult> mockResultsByURL = new Dictionary<Uri, IWebClientResult>();
            const int numPages = 3;
            for (int page = 1; page <= numPages; page++)
            {
                // Avoid capturing page variable in lambda below, which will cause it to attempt
                // to get page numPages + 1
                int pageCopy = page;
                string url = $"https://www.hulu.com/start/more_content?channel=anime&video_type=all&sort=alpha&is_current=0&closed_captioned=0&has_hd=0&page={page}";

                var resultMock = new Mock<IWebClientResult>();
                resultMock.Setup(r => r.ReadResponseAsStringAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(GetResourceText(pageCopy)));
                resultMock.Setup(r => r.Dispose()).Callback(() => { });
                mockResultsByURL[new Uri(url)] = resultMock.Object;
            }

            webClientMock.Setup(client => client.GetAsync(It.IsAny<WebClientRequest>(), It.IsAny<CancellationToken>())).Returns<WebClientRequest, CancellationToken>((request, token) => Task.FromResult(mockResultsByURL[request.URL]));
            return webClientMock.Object;
        }

        private static List<AnimeStreamInfo> ExpectedFirstPageStreamInfo = new List<AnimeStreamInfo>()
        {
            new AnimeStreamInfo("Absolute Duo", "https://www.hulu.com/absolute-duo", StreamingService.Hulu),
            new AnimeStreamInfo("Accel World", "https://www.hulu.com/accel-world", StreamingService.Hulu),
            new AnimeStreamInfo("Aesthetica of a Rogue Hero", "https://www.hulu.com/aesthetica-of-a-rogue-hero", StreamingService.Hulu),
            new AnimeStreamInfo("Afro Samurai", "https://www.hulu.com/afro-samurai", StreamingService.Hulu),
            new AnimeStreamInfo("Afro Samurai Resurrection", "https://www.hulu.com/afro-samurai-resurrection", StreamingService.Hulu),
            new AnimeStreamInfo("Air Gear", "https://www.hulu.com/air-gear", StreamingService.Hulu),
            new AnimeStreamInfo("Akame ga Kill!", "https://www.hulu.com/akame-ga-kill", StreamingService.Hulu),
            new AnimeStreamInfo("Akane Iro ni Somaru Saka", "https://www.hulu.com/akane-iro-ni-somaru-saka", StreamingService.Hulu),
            new AnimeStreamInfo("Akira", "https://www.hulu.com/akira", StreamingService.Hulu),
            new AnimeStreamInfo("ALDNOAH.ZERO", "https://www.hulu.com/aldnoahzero", StreamingService.Hulu),
            new AnimeStreamInfo("Amagami SS", "https://www.hulu.com/amagami-ss", StreamingService.Hulu),
            new AnimeStreamInfo("Amagi Brilliant Park", "https://www.hulu.com/amagi-brilliant-park", StreamingService.Hulu),
            new AnimeStreamInfo("The Ambition of Oda Nobuna", "https://www.hulu.com/the-ambition-of-oda-nobuna", StreamingService.Hulu),
            new AnimeStreamInfo("Angels of Death", "https://www.hulu.com/angels-of-death", StreamingService.Hulu),
            new AnimeStreamInfo("Another", "https://www.hulu.com/another", StreamingService.Hulu),
            new AnimeStreamInfo("Aoharu x Machinegun", "https://www.hulu.com/aoharu-x-machinegun", StreamingService.Hulu),
            new AnimeStreamInfo("Aquarion", "https://www.hulu.com/aquarion", StreamingService.Hulu),
            new AnimeStreamInfo("Aquarion EVOL", "https://www.hulu.com/aquarion-evol", StreamingService.Hulu),
            new AnimeStreamInfo("Aria: The Scarlet Ammo", "https://www.hulu.com/aria-the-scarlet-ammo", StreamingService.Hulu),
            new AnimeStreamInfo("Assassination Classroom", "https://www.hulu.com/assassination-classroom", StreamingService.Hulu),
            new AnimeStreamInfo("The Asterisk War", "https://www.hulu.com/the-asterisk-war", StreamingService.Hulu),
            new AnimeStreamInfo("Astro Boy", "https://www.hulu.com/astro-boy", StreamingService.Hulu),
            new AnimeStreamInfo("Astro Boy en Español", "https://www.hulu.com/astro-boy-en-espanol", StreamingService.Hulu),
            new AnimeStreamInfo("Atelier Escha & Logy: Alchemists of the Dusk Sky", "https://www.hulu.com/atelier-escha-and-logy-alchemists-of-the-dusk-sky", StreamingService.Hulu),
        };

        private static List<AnimeStreamInfo> ExpectedStreamInfo = new List<AnimeStreamInfo>()
        {
            new AnimeStreamInfo("Absolute Duo", "https://www.hulu.com/absolute-duo", StreamingService.Hulu),
            new AnimeStreamInfo("Accel World", "https://www.hulu.com/accel-world", StreamingService.Hulu),
            new AnimeStreamInfo("Aesthetica of a Rogue Hero", "https://www.hulu.com/aesthetica-of-a-rogue-hero", StreamingService.Hulu),
            new AnimeStreamInfo("Afro Samurai", "https://www.hulu.com/afro-samurai", StreamingService.Hulu),
            new AnimeStreamInfo("Afro Samurai Resurrection", "https://www.hulu.com/afro-samurai-resurrection", StreamingService.Hulu),
            new AnimeStreamInfo("Air Gear", "https://www.hulu.com/air-gear", StreamingService.Hulu),
            new AnimeStreamInfo("Akame ga Kill!", "https://www.hulu.com/akame-ga-kill", StreamingService.Hulu),
            new AnimeStreamInfo("Akane Iro ni Somaru Saka", "https://www.hulu.com/akane-iro-ni-somaru-saka", StreamingService.Hulu),
            new AnimeStreamInfo("Akira", "https://www.hulu.com/akira", StreamingService.Hulu),
            new AnimeStreamInfo("ALDNOAH.ZERO", "https://www.hulu.com/aldnoahzero", StreamingService.Hulu),
            new AnimeStreamInfo("Amagami SS", "https://www.hulu.com/amagami-ss", StreamingService.Hulu),
            new AnimeStreamInfo("Amagi Brilliant Park", "https://www.hulu.com/amagi-brilliant-park", StreamingService.Hulu),
            new AnimeStreamInfo("The Ambition of Oda Nobuna", "https://www.hulu.com/the-ambition-of-oda-nobuna", StreamingService.Hulu),
            new AnimeStreamInfo("Angels of Death", "https://www.hulu.com/angels-of-death", StreamingService.Hulu),
            new AnimeStreamInfo("Another", "https://www.hulu.com/another", StreamingService.Hulu),
            new AnimeStreamInfo("Aoharu x Machinegun", "https://www.hulu.com/aoharu-x-machinegun", StreamingService.Hulu),
            new AnimeStreamInfo("Aquarion", "https://www.hulu.com/aquarion", StreamingService.Hulu),
            new AnimeStreamInfo("Aquarion EVOL", "https://www.hulu.com/aquarion-evol", StreamingService.Hulu),
            new AnimeStreamInfo("Aria: The Scarlet Ammo", "https://www.hulu.com/aria-the-scarlet-ammo", StreamingService.Hulu),
            new AnimeStreamInfo("Assassination Classroom", "https://www.hulu.com/assassination-classroom", StreamingService.Hulu),
            new AnimeStreamInfo("The Asterisk War", "https://www.hulu.com/the-asterisk-war", StreamingService.Hulu),
            new AnimeStreamInfo("Astro Boy", "https://www.hulu.com/astro-boy", StreamingService.Hulu),
            new AnimeStreamInfo("Astro Boy en Español", "https://www.hulu.com/astro-boy-en-espanol", StreamingService.Hulu),
            new AnimeStreamInfo("Atelier Escha & Logy: Alchemists of the Dusk Sky", "https://www.hulu.com/atelier-escha-and-logy-alchemists-of-the-dusk-sky", StreamingService.Hulu),

            new AnimeStreamInfo("Yu-Gi-Oh! 5D's", "https://www.hulu.com/yu-gi-oh-5ds", StreamingService.Hulu),
            new AnimeStreamInfo("Yu-Gi-Oh! ARC-V", "https://www.hulu.com/yugioh-arc-v", StreamingService.Hulu),
            new AnimeStreamInfo("Yu-Gi-Oh! GX", "https://www.hulu.com/yu-gi-oh-gx", StreamingService.Hulu),
            new AnimeStreamInfo("Yu-Gi-Oh! ZEXAL", "https://www.hulu.com/yu-gi-oh-zexal", StreamingService.Hulu),
            new AnimeStreamInfo("Yu Yu Hakusho", "https://www.hulu.com/yu-yu-hakusho", StreamingService.Hulu),
            new AnimeStreamInfo("Zatch Bell!", "https://www.hulu.com/zatch-bell", StreamingService.Hulu),
            new AnimeStreamInfo("Zetman", "https://www.hulu.com/zetman", StreamingService.Hulu),
        };

        private static string GetResourceText(int page)
        {
            string resourceShortName = string.Format("hulu_{0}.html", page);
            return Helpers.GetResourceText(resourceShortName);
        }
    }
}
