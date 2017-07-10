using AnimeRecs.DAL;
using Moq;
using System;
using System.Collections.Generic;
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
    public class AmazonPrimeStreamInfoSourceTests
    {
        [Fact]
        public void TestGetAnimeStreamInfo()
        {
            IWebClient mockWebClient = GetMockWebClient();
            AmazonPrimeStreamInfoSource source = new AmazonPrimeStreamInfoSource(mockWebClient);
            ICollection<AnimeStreamInfo> streams = source.GetAnimeStreamInfoAsync(CancellationToken.None).GetAwaiter().GetResult();
            streams.Should().BeEquivalentTo(ExpectedStreamInfo);
        }

        private static IWebClient GetMockWebClient()
        {
            var webClientMock = new Mock<IWebClient>();

            Dictionary<string, string> resourceNamesOfMockPages = new Dictionary<string, string>();
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_rot_p_n_ways_to_watch_1?rh=n:2858778011,p_n_theme_browse-bin:2650364011,p_n_ways_to_watch:12007865011&ie=UTF8"] = "amazon_prime_1.html";
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_pg_2?rh=n:2858778011%2Cp_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011&page=2&ie=UTF8&qid=1495396353"] = "amazon_prime_2.html";
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_pg_6?rh=n:2858778011%2Cp_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011&page=6&ie=UTF8&qid=1495396393"] = "amazon_prime_3.html";

            Dictionary<Uri, IWebClientResult> mockResultsByURL = new Dictionary<Uri, IWebClientResult>();
            foreach (string url in resourceNamesOfMockPages.Keys)
            {
                string urlCopy = url;

                var resultMock = new Mock<IWebClientResult>();
                resultMock.Setup(r => r.ReadResponseAsStringAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(Helpers.GetResourceText(resourceNamesOfMockPages[urlCopy])));
                resultMock.Setup(r => r.Dispose()).Callback(() => { });
                mockResultsByURL[new Uri(url)] = resultMock.Object;
            }

            webClientMock.Setup(client => client.GetAsync(It.IsAny<WebClientRequest>(), It.IsAny<CancellationToken>())).Returns<WebClientRequest, CancellationToken>((request, token) => Task.FromResult(mockResultsByURL[request.URL]));
            return webClientMock.Object;
        }

        private static List<AnimeStreamInfo> ExpectedStreamInfo = new List<AnimeStreamInfo>()
        {
            new AnimeStreamInfo("The Legend of Korra Book 1", "https://www.amazon.com/A-Leaf-in-the-Wind/dp/B007QYRCX6", StreamingService.AmazonPrime),
            new AnimeStreamInfo("The Legend of Korra Book 3", "https://www.amazon.com/A-Breath-of-Fresh-Air/dp/B00LEXEPJ4", StreamingService.AmazonPrime),
            new AnimeStreamInfo("The Legend of Korra Book 2", "https://www.amazon.com/The-Southern-Lights/dp/B00F6PKYX2", StreamingService.AmazonPrime),
            new AnimeStreamInfo("ThunderCats (Original Series): The Complete First Season, Volume 1", "https://www.amazon.com/Exodus/dp/B005C8DB7E", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Legend Of the Millennium Dragon", "https://www.amazon.com/Legend-Millennium-Dragon-Kensho-Ono/dp/B06ZY384GY", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Mix Master: Final Force", "https://www.amazon.com/The-Mix-Master-is-Back/dp/B06XR5XD17", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Tenkai Knights - Rise of the Knights", "https://www.amazon.com/Tenkai-Knights-Rise-Brian-Beacock/dp/B00I8H72YY", StreamingService.AmazonPrime),
            new AnimeStreamInfo("KABANERI OF THE IRON FORTRESS(Subbed)", "https://www.amazon.com/Prayer-Offer/dp/B01DU4K5ES", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Elfen Lied - Season 01", "https://www.amazon.com/Elfen-Lied-01-Chance-Encounter/dp/B003B21SP0", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Broken Blade Season 1", "https://www.amazon.com/The-Time-of-Awakening/dp/B00AI91ODA", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Technotise: Edit and I", "https://www.amazon.com/Technotise-Edit-I-Sanda-Knezevic/dp/B06XRFPVQ5", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Robotech: The Complete Series - Digitally Remastered", "https://www.amazon.com/The-Macross-Saga-Boobytrap/dp/B01I4ICUAC", StreamingService.AmazonPrime),
            new AnimeStreamInfo("ThunderCats (Original Series): The Complete First Season, Volume 2", "https://www.amazon.com/Queen-of-8-Legs/dp/B005C8DDW2", StreamingService.AmazonPrime),
            new AnimeStreamInfo("UFO Ultramaiden", "https://www.amazon.com/UFO-Ultramaiden-01-Bathhouse-Angel/dp/B00425YJIS", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Mix Master: King of Cards", "https://www.amazon.com/Mix-Master-Cards-Season-Legend/dp/B00AVTPJ4C", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Get Backers", "https://www.amazon.com/Initials-Are-G-B/dp/B003E33S9A", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Demon King Daimao Season 1 (English Subtitled)", "https://www.amazon.com/The-Demon-King-Is-Born/dp/B00DAA5ZPW", StreamingService.AmazonPrime),
            new AnimeStreamInfo("ThunderCats (Original Series): The Complete Second Season, Volume 1", "https://www.amazon.com/ThunderCats-Ho-Part-1/dp/B005HGA9DA", StreamingService.AmazonPrime),
            new AnimeStreamInfo("RahXephon - Season 01", "https://www.amazon.com/RahXephon-01-Invasion-Capital/dp/B003B1VJ0U", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Little Snow Fairy Sugar", "https://www.amazon.com/Little-Snow-Fairy-Sugar-Meets/dp/B0040G8GTW", StreamingService.AmazonPrime),
            new AnimeStreamInfo("ThunderCats (Original Series): The Complete Second Season, Volume 2", "https://www.amazon.com/Exile-Isle/dp/B005HE8AWY", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Princess Tutu - Season 01", "https://www.amazon.com/Princess-Tutu-01-Duck-Prince/dp/B003B1VIXI", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Battle Disc", "https://www.amazon.com/Horrible-Golden-Family/dp/B01M0D2IDK", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Mythical Detective Loki Ragnarok", "https://www.amazon.com/Mythical-Detective-Loki-Ragnarok-01/dp/B004FJTWJM", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Delinquent Hamsters", "https://www.amazon.com/Delinquent-Hamsters/dp/B06XDWKFF6", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Amethyst - Princess of Gem World", "https://www.amazon.com/Village-of-the-Frogs/dp/B01J71JF1C", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Orphen Season 1", "https://www.amazon.com/Orphen-01-Sword-Baltanders/dp/B008P95CBA", StreamingService.AmazonPrime),
            new AnimeStreamInfo("The Mystical Laws", "https://www.amazon.com/Mystical-Laws-Isamu-Imakake/dp/B00TJZ2O2Y", StreamingService.AmazonPrime),
            new AnimeStreamInfo("UFO Ultramaiden Season 2", "https://www.amazon.com/UFO-Ultramaiden-01-Key-Time/dp/B0041WL02K", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Robotech: The Complete Series - Digitally Remastered", "https://www.amazon.com/The-Masters-Saga-Danas-Story/dp/B01I4F5L9M", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Robotech: The Shadow Chronicles", "https://www.amazon.com/Robotech-Shadow-Chronicles-Mark-Hamill/dp/B01HPMWXU0", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Swiss Family Robinson", "https://www.amazon.com/The-Adventure-Begins/dp/B019EFG1MC", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Clip: The travel of a commuter.", "https://www.amazon.com/Clip-travel-commuter-Shigeo-Fukushima/dp/B01MYRDR43", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Game-Con Haul! My Loot Bag!", "https://www.amazon.com/Game-Con-Haul-My-Loot-Bag/dp/B01HSTE5NI", StreamingService.AmazonPrime)
        };
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
