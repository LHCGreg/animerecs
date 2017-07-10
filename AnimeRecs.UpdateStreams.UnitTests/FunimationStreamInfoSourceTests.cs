using AnimeRecs.DAL;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Threading;

namespace AnimeRecs.UpdateStreams.UnitTests
{
    public class FunimationStreamInfoSourceTests
    {
        [Fact]
        public void TestHelperStreamInfoSource()
        {
            IWebClient mockWebClient = GetMockWebClient();
            FunimationStreamInfoSource.HelperStreamInfoSource source = new FunimationStreamInfoSource.HelperStreamInfoSource(mockWebClient, "https://www.funimation.com/shows/all-shows/?sort=show&p=1");
            ICollection<AnimeStreamInfo> streams = source.GetAnimeStreamInfoAsync(CancellationToken.None).GetAwaiter().GetResult();
            streams.Should().BeEquivalentTo(ExpectedFirstPageStreamInfo);
        }

        [Fact]
        public void TestHelperStreamInfoSourceOnEmpty()
        {
            IWebClient mockWebClient = GetMockWebClient();
            FunimationStreamInfoSource.HelperStreamInfoSource source = new FunimationStreamInfoSource.HelperStreamInfoSource(mockWebClient, "https://www.funimation.com/shows/all-shows/?sort=show&p=5");
            Assert.Throws<NoMatchingHtmlException>(() => source.GetAnimeStreamInfoAsync(CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void TestGetAnimeStreamInfo()
        {
            IWebClient mockWebClient = GetMockWebClient();
            FunimationStreamInfoSource source = new FunimationStreamInfoSource(mockWebClient);
            ICollection<AnimeStreamInfo> streams = source.GetAnimeStreamInfoAsync(CancellationToken.None).GetAwaiter().GetResult();
            streams.Should().BeEquivalentTo(ExpectedStreamInfo);
        }

        private static IWebClient GetMockWebClient()
        {
            var webClientMock = new Mock<IWebClient>();

            Dictionary<Uri, IWebClientResult> mockResultsByURL = new Dictionary<Uri, IWebClientResult>();
            const int numPages = 5;
            for (int page = 1; page <= numPages; page++)
            {
                // Avoid capturing page variable in lambda below, which will cause it to attempt
                // to get page numPages + 1
                int pageCopy = page;
                string url = string.Format("https://www.funimation.com/shows/all-shows/?sort=show&p={0}", page);

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
            new AnimeStreamInfo("009 Re:Cyborg", "https://www.funimation.com/shows/009-recyborg/", StreamingService.Funimation),
            new AnimeStreamInfo("91 Days", "https://www.funimation.com/shows/91-days/", StreamingService.Funimation),
            new AnimeStreamInfo("A Certain Magical Index", "https://www.funimation.com/shows/a-certain-magical-index/", StreamingService.Funimation),
            new AnimeStreamInfo("A Certain Scientific Railgun", "https://www.funimation.com/shows/a-certain-scientific-railgun/", StreamingService.Funimation),
            new AnimeStreamInfo("A good librarian like a good shepherd", "https://www.funimation.com/shows/a-good-librarian-like-a-good-shepherd/", StreamingService.Funimation),
            new AnimeStreamInfo("Absolute Duo", "https://www.funimation.com/shows/absolute-duo/", StreamingService.Funimation),
            new AnimeStreamInfo("ACCA: 13-Territory Inspection Dept.", "https://www.funimation.com/shows/acca/", StreamingService.Funimation),
            new AnimeStreamInfo("Aesthetica of a Rogue Hero", "https://www.funimation.com/shows/aesthetica-of-a-rogue-hero/", StreamingService.Funimation),
            new AnimeStreamInfo("Afro Samurai", "https://www.funimation.com/shows/afro-samurai/", StreamingService.Funimation),
            new AnimeStreamInfo("Ah! My Goddess: Flights of Fancy", "https://www.funimation.com/shows/ah-my-goddess-flights-of-fancy/", StreamingService.Funimation),
            new AnimeStreamInfo("Ai Yori Aoshi", "https://www.funimation.com/shows/ai-yori-aoshi/", StreamingService.Funimation),
            new AnimeStreamInfo("Air Gear", "https://www.funimation.com/shows/air-gear/", StreamingService.Funimation),
            new AnimeStreamInfo("Akiba's Trip The Animation", "https://www.funimation.com/shows/akibas-trip-the-animation/", StreamingService.Funimation),
            new AnimeStreamInfo("Akira", "https://www.funimation.com/shows/akira/", StreamingService.Funimation),
            new AnimeStreamInfo("Alderamin on the Sky", "https://www.funimation.com/shows/alderamin-on-the-sky/", StreamingService.Funimation),
            new AnimeStreamInfo("Alien vs Ninja", "https://www.funimation.com/shows/alien-vs-ninja/", StreamingService.Funimation),
            new AnimeStreamInfo("ALL OUT!!", "https://www.funimation.com/shows/all-out/", StreamingService.Funimation),
            new AnimeStreamInfo("And you thought there is never a girl online?", "https://www.funimation.com/shows/and-you-thought-there-is-never-a-girl-online/", StreamingService.Funimation),
            new AnimeStreamInfo("Appleseed XIII", "https://www.funimation.com/shows/appleseed-xiii/", StreamingService.Funimation),
            new AnimeStreamInfo("Aquarion", "https://www.funimation.com/shows/aquarion/", StreamingService.Funimation),
        };

        private static List<AnimeStreamInfo> ExpectedStreamInfo = new List<AnimeStreamInfo>()
        {
            new AnimeStreamInfo("009 Re:Cyborg", "https://www.funimation.com/shows/009-recyborg/", StreamingService.Funimation),
            new AnimeStreamInfo("91 Days", "https://www.funimation.com/shows/91-days/", StreamingService.Funimation),
            new AnimeStreamInfo("A Certain Magical Index", "https://www.funimation.com/shows/a-certain-magical-index/", StreamingService.Funimation),
            new AnimeStreamInfo("A Certain Scientific Railgun", "https://www.funimation.com/shows/a-certain-scientific-railgun/", StreamingService.Funimation),
            new AnimeStreamInfo("A good librarian like a good shepherd", "https://www.funimation.com/shows/a-good-librarian-like-a-good-shepherd/", StreamingService.Funimation),
            new AnimeStreamInfo("Absolute Duo", "https://www.funimation.com/shows/absolute-duo/", StreamingService.Funimation),
            new AnimeStreamInfo("ACCA: 13-Territory Inspection Dept.", "https://www.funimation.com/shows/acca/", StreamingService.Funimation),
            new AnimeStreamInfo("Aesthetica of a Rogue Hero", "https://www.funimation.com/shows/aesthetica-of-a-rogue-hero/", StreamingService.Funimation),
            new AnimeStreamInfo("Afro Samurai", "https://www.funimation.com/shows/afro-samurai/", StreamingService.Funimation),
            new AnimeStreamInfo("Ah! My Goddess: Flights of Fancy", "https://www.funimation.com/shows/ah-my-goddess-flights-of-fancy/", StreamingService.Funimation),
            new AnimeStreamInfo("Ai Yori Aoshi", "https://www.funimation.com/shows/ai-yori-aoshi/", StreamingService.Funimation),
            new AnimeStreamInfo("Air Gear", "https://www.funimation.com/shows/air-gear/", StreamingService.Funimation),
            new AnimeStreamInfo("Akiba's Trip The Animation", "https://www.funimation.com/shows/akibas-trip-the-animation/", StreamingService.Funimation),
            new AnimeStreamInfo("Akira", "https://www.funimation.com/shows/akira/", StreamingService.Funimation),
            new AnimeStreamInfo("Alderamin on the Sky", "https://www.funimation.com/shows/alderamin-on-the-sky/", StreamingService.Funimation),
            new AnimeStreamInfo("Alien vs Ninja", "https://www.funimation.com/shows/alien-vs-ninja/", StreamingService.Funimation),
            new AnimeStreamInfo("ALL OUT!!", "https://www.funimation.com/shows/all-out/", StreamingService.Funimation),
            new AnimeStreamInfo("And you thought there is never a girl online?", "https://www.funimation.com/shows/and-you-thought-there-is-never-a-girl-online/", StreamingService.Funimation),
            new AnimeStreamInfo("Appleseed XIII", "https://www.funimation.com/shows/appleseed-xiii/", StreamingService.Funimation),
            new AnimeStreamInfo("Aquarion", "https://www.funimation.com/shows/aquarion/", StreamingService.Funimation),

            new AnimeStreamInfo("Aquarion EVOL", "https://www.funimation.com/shows/aquarion-evol/", StreamingService.Funimation),
            new AnimeStreamInfo("Aquarion Logos", "https://www.funimation.com/shows/aquarion-logos/", StreamingService.Funimation),
            new AnimeStreamInfo("Aria the Scarlet Ammo", "https://www.funimation.com/shows/aria-the-scarlet-ammo/", StreamingService.Funimation),
            new AnimeStreamInfo("Armitage III", "https://www.funimation.com/shows/armitage-iii/", StreamingService.Funimation),
            new AnimeStreamInfo("Assassination Classroom", "https://www.funimation.com/shows/assassination-classroom/", StreamingService.Funimation),
            new AnimeStreamInfo("Athena: Goddess of War", "https://www.funimation.com/shows/athena-goddess-of-war/", StreamingService.Funimation),
            new AnimeStreamInfo("Attack on Titan", "https://www.funimation.com/shows/attack-on-titan/", StreamingService.Funimation),
            new AnimeStreamInfo("Attack on Titan: Junior High", "https://www.funimation.com/shows/attack-on-titan-junior-high/", StreamingService.Funimation),
            new AnimeStreamInfo("Baka & Test - Summon the Beasts -", "https://www.funimation.com/shows/baka-test-summon-the-beasts/", StreamingService.Funimation),
            new AnimeStreamInfo("Baldr Force Exe", "https://www.funimation.com/shows/baldr-force-exe/", StreamingService.Funimation),
            new AnimeStreamInfo("Bamboo Blade", "https://www.funimation.com/shows/bamboo-blade/", StreamingService.Funimation),
            new AnimeStreamInfo("Barakamon", "https://www.funimation.com/shows/barakamon/", StreamingService.Funimation),
            new AnimeStreamInfo("Basilisk", "https://www.funimation.com/shows/basilisk/", StreamingService.Funimation),
            new AnimeStreamInfo("BAYONETTA: Bloody Fate", "https://www.funimation.com/shows/bayonetta-bloody-fate/", StreamingService.Funimation),
            new AnimeStreamInfo("Ben-To", "https://www.funimation.com/shows/ben-to/", StreamingService.Funimation),
            new AnimeStreamInfo("Big Windup!", "https://www.funimation.com/shows/big-windup/", StreamingService.Funimation),
            new AnimeStreamInfo("Bikini Warriors", "https://www.funimation.com/shows/bikini-warriors/", StreamingService.Funimation),
            new AnimeStreamInfo("Black Blood Brothers", "https://www.funimation.com/shows/black-blood-brothers/", StreamingService.Funimation),
            new AnimeStreamInfo("Black Butler", "https://www.funimation.com/shows/black-butler/", StreamingService.Funimation),
            new AnimeStreamInfo("Black Cat", "https://www.funimation.com/shows/black-cat/", StreamingService.Funimation),

            new AnimeStreamInfo("Touken Ranbu Hanamaru", "https://www.funimation.com/shows/touken-ranbu-hanamaru/", StreamingService.Funimation),
            new AnimeStreamInfo("Tower of Druaga", "https://www.funimation.com/shows/tower-of-druaga/", StreamingService.Funimation),
            new AnimeStreamInfo("Trickster", "https://www.funimation.com/shows/trickster/", StreamingService.Funimation),
            new AnimeStreamInfo("Trigun", "https://www.funimation.com/shows/trigun/", StreamingService.Funimation),
            new AnimeStreamInfo("Trinity Blood", "https://www.funimation.com/shows/trinity-blood/", StreamingService.Funimation),
            new AnimeStreamInfo("Tsubasa RESERVoir CHRoNiCLE", "https://www.funimation.com/shows/tsubasa-reservoir-chronicle/", StreamingService.Funimation),
            new AnimeStreamInfo("TSUKIUTA. The Animation", "https://www.funimation.com/shows/tsukiuta-the-animation/", StreamingService.Funimation),
            new AnimeStreamInfo("UFO Ultramaiden Valkyrie", "https://www.funimation.com/shows/ufo-ultramaiden-valkyrie/", StreamingService.Funimation),
            new AnimeStreamInfo("Ultimate Otaku Teacher", "https://www.funimation.com/shows/ultimate-otaku-teacher/", StreamingService.Funimation),
            new AnimeStreamInfo("Unbreakable Machine-Doll", "https://www.funimation.com/shows/unbreakable-machine-doll/", StreamingService.Funimation),
            new AnimeStreamInfo("Utawarerumono", "https://www.funimation.com/shows/utawarerumono/", StreamingService.Funimation),
            new AnimeStreamInfo("Valkyrie Drive -Mermaid-", "https://www.funimation.com/shows/valkyrie-drive-mermaid-/", StreamingService.Funimation),
            new AnimeStreamInfo("Vampire Girl vs Frankenstein Girl", "https://www.funimation.com/shows/vampire-girl-vs-frankenstein-girl/", StreamingService.Funimation),
            new AnimeStreamInfo("Vandread", "https://www.funimation.com/shows/vandread/", StreamingService.Funimation),
            new AnimeStreamInfo("Venus Project -Climax-", "https://www.funimation.com/shows/venus-project-climax-/", StreamingService.Funimation),
            new AnimeStreamInfo("Venus Versus Virus", "https://www.funimation.com/shows/venus-versus-virus/", StreamingService.Funimation),
            new AnimeStreamInfo("Vexille", "https://www.funimation.com/shows/vexille/", StreamingService.Funimation),
            new AnimeStreamInfo("Wanna be the Strongest in the World!", "https://www.funimation.com/shows/wanna-be-the-strongest-in-the-world/", StreamingService.Funimation),
            new AnimeStreamInfo("We Without Wings", "https://www.funimation.com/shows/we-without-wings/", StreamingService.Funimation),
            new AnimeStreamInfo("Welcome to the N-H-K", "https://www.funimation.com/shows/welcome-to-the-n-h-k/", StreamingService.Funimation),

            new AnimeStreamInfo("Witchblade", "https://www.funimation.com/shows/witchblade/", StreamingService.Funimation),
            new AnimeStreamInfo("Wolf's Rain", "https://www.funimation.com/shows/wolfs-rain/", StreamingService.Funimation),
            new AnimeStreamInfo("World Break: Aria of Curse for a Holy Swordsman", "https://www.funimation.com/shows/world-break-aria-of-curse-for-a-holy-swordsman/", StreamingService.Funimation),
            new AnimeStreamInfo("X", "https://www.funimation.com/shows/x/", StreamingService.Funimation),
            new AnimeStreamInfo("xxxHOLiC", "https://www.funimation.com/shows/xxxholic/", StreamingService.Funimation),
            new AnimeStreamInfo("Yamada's First Time: B Gata H Kei", "https://www.funimation.com/shows/yamadas-first-time-b-gata-h-kei/", StreamingService.Funimation),
            new AnimeStreamInfo("Yatterman Night", "https://www.funimation.com/shows/yatterman-night/", StreamingService.Funimation),
            new AnimeStreamInfo("Yona of the Dawn", "https://www.funimation.com/shows/yona-of-the-dawn/", StreamingService.Funimation),
            new AnimeStreamInfo("Yu Yu Hakusho", "https://www.funimation.com/shows/yu-yu-hakusho/", StreamingService.Funimation),
            new AnimeStreamInfo("Yuri!!! On ICE", "https://www.funimation.com/shows/yuri-on-ice/", StreamingService.Funimation),
            new AnimeStreamInfo("Yurikuma Arashi", "https://www.funimation.com/shows/yurikuma-arashi/", StreamingService.Funimation),
            new AnimeStreamInfo("Zebraman 2", "https://www.funimation.com/shows/zebraman-2/", StreamingService.Funimation),
        };

        private static string GetResourceText(int page)
        {
            string resourceShortName = string.Format("funimation_{0}.html", page);
            return Helpers.GetResourceText(resourceShortName);
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

