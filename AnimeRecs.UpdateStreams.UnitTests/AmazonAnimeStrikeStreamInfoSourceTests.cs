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

namespace AnimeRecs.UpdateStreams.UnitTests
{
    public class AmazonAnimeStrikeStreamInfoSourceTests
    {
        [Fact]
        public void TestGetAnimeStreamInfo()
        {
            IWebClient mockWebClient = GetMockWebClient();
            AmazonAnimeStrikeStreamInfoSource source = new AmazonAnimeStrikeStreamInfoSource(mockWebClient);
            ICollection<AnimeStreamInfo> streams = source.GetAnimeStreamInfo();
            streams.Should().BeEquivalentTo(ExpectedStreamInfo);
        }

        private static IWebClient GetMockWebClient()
        {
            var webClientMock = new Mock<IWebClient>();

            Dictionary<string, string> resourceNamesOfMockPages = new Dictionary<string, string>();
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_nr_p_n_subscription_id_1?rh=n:2858778011,p_n_ways_to_watch:12007866011,p_n_subscription_id:16182082011&ie=UTF8"] = "amazon_anime_strike_1.html";
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_pg_2?rh=n:2858778011%2Cp_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011&page=2&ie=UTF8&qid=1495396159"] = "amazon_anime_strike_2.html";
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_pg_9?rh=n:2858778011%2Cp_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011&page=9&ie=UTF8&qid=1495396218"] = "amazon_anime_strike_3.html";

            Dictionary<Uri, IWebClientResult> mockResultsByURL = new Dictionary<Uri, IWebClientResult>();
            foreach (string url in resourceNamesOfMockPages.Keys)
            {
                string urlCopy = url;

                var resultMock = new Mock<IWebClientResult>();
                resultMock.Setup(r => r.ReadResponseAsString()).Returns(Helpers.GetResourceText(resourceNamesOfMockPages[urlCopy]));
                resultMock.Setup(r => r.Dispose()).Callback(() => { });
                mockResultsByURL[new Uri(url)] = resultMock.Object;
            }

            webClientMock.Setup(client => client.Get(It.IsAny<WebClientRequest>())).Returns<WebClientRequest>(request => mockResultsByURL[request.URL]);
            return webClientMock.Object;
        }

        private static List<AnimeStreamInfo> ExpectedStreamInfo = new List<AnimeStreamInfo>()
        {
            new AnimeStreamInfo("Armed Girl's Machiavellism - Season 1", "https://www.amazon.com/The-Dubious-Sword-Satori-Tamaba/dp/B06Y1Q4SM7", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Grimoire of Zero - Season 1", "https://www.amazon.com/Thirteen/dp/B06Y3CSGF1", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Re:CREATORS", "https://www.amazon.com/You-knows-where-justice-lies/dp/B06Y3WYQHG", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Sword Oratoria: Is it Wrong to Try to Pick Up Girls in a Dungeon? On the Side - Season 1", "https://www.amazon.com/Red-head-Lone-Ruler/dp/B071QY12G4", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("RAGE OF BAHAMUT VIRGIN SOUL", "https://www.amazon.com/Anatean-Holiday/dp/B06Y3LMLGW", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Anonymous Noise - Season 1", "https://www.amazon.com/keep-Walking-Forward-Today-Tomorrow/dp/B06Y5SCYK4", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Saekano- How to Raise a Boring Girlfriend .flat", "https://www.amazon.com/Episode5-Deadline-or-Awakening/dp/B06Y1X4HL1", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Akira (English Dubbed)", "https://www.amazon.com/Akira-English-Dubbed-Katsuhiro-Ohtomo/dp/B00G0AHDKE", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Sagrada Reset - Season 1", "https://www.amazon.com/WITCH-PICTURE-RED-EYE-GIRL/dp/B06Y1R8D86", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Paprika", "https://www.amazon.com/Paprika-Megumi-Hayashibara/dp/B00CX8O0NU", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("GRANBLUE FANTASY The Animation", "https://www.amazon.com/The-Iron-Giant/dp/B01MYASHGC", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Darker Than Black Season 1 (English Dubbed)", "https://www.amazon.com/Fallen-Star-Contract-Part/dp/B002S45X62", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Eromanga Sensei - Season 1", "https://www.amazon.com/Masamune-Izumi-Nemesis-Million-Copies/dp/B06Y4CWQJB", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Fullmetal Alchemist Brotherhood (English Dubbed) Season 1", "https://www.amazon.com/Fullmetal-Alchemist/dp/B003E3XIDQ", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Is It Wrong to Try to Pick Up Girls In a Dungeon? Season 1", "https://www.amazon.com/Bell-Cranel-ADVENTURER/dp/B01N0YCDM9", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Code Geass: Lelouch of the Rebellion, Season 1 (Original Japanese Version)", "https://www.amazon.com/Stage-01-Day-Demon-Born/dp/B06Y4FPL4X", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Atom: the Beginning - Season 1", "https://www.amazon.com/Step-on-it-Maruhige-Shipping/dp/B071Y1GY5P", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("One-Punch Man- Season 1", "https://www.amazon.com/The-Strongest-Man/dp/B01N4N4K4F", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("High School of the Dead Season 1 (English Dubbed)", "https://www.amazon.com/Spring-of-the-DEAD/dp/B06X92DXYF", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Scum's Wish", "https://www.amazon.com/Scums-Wish/dp/B01NCUV2FV", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Parasyte - the maxim - Season 1 (English Dubbed)", "https://www.amazon.com/The-Metamorphosis/dp/B0166I3GO6", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Black Bullet Season 1 (English Dubbed)", "https://www.amazon.com/The-Last-Hope/dp/B01NARYWVR", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Akame ga Kill! Season 1 (English Dubbed)", "https://www.amazon.com/Kill-the-Darkness/dp/B06VVNXLGT", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("No Game, No Life Season 1 (English Dubbed)", "https://www.amazon.com/Beginner/dp/B01NBTFPUH", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Darker Than Black", "https://www.amazon.com/Fallen-Star-Contract-Part/dp/B071H4RCZP", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Full Metal Panic! Season 1 (English Dubbed)", "https://www.amazon.com/Guy-Kinda-Like-Sergeant/dp/B0047C8LH6", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Blue Exorcist- Kyoto Saga", "https://www.amazon.com/Small-Beginnings/dp/B01N3384UP", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Vampire Hunter D", "https://www.amazon.com/Vampire-Hunter-D-Toyoo-Ashida/dp/B01N20N3LT", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Ergo Proxy Season 1 (English Dubbed)", "https://www.amazon.com/Awakening/dp/B002SEFWZY", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("ONIHEI", "https://www.amazon.com/Tanbei-of-Chigashira/dp/B01MR4S1XJ", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Soul Eater (English Dubbed)", "https://www.amazon.com/Resonance-Soul-Eater-Become-Scythe/dp/B0037OUWMW", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Maid-Sama! Season 1 (English Dubbed)", "https://www.amazon.com/Misa-is-a-Maid-Sama/dp/B01MT7TT7L", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("K - Season 1", "https://www.amazon.com/Knight/dp/B01N4MDEX6", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Clannad: After Story", "https://www.amazon.com/Farewell-End-Summer/dp/B01N7QJKJC", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Puella Magi Madoka Magica Season 1", "https://www.amazon.com/First-Met-Her-Dream-Something/dp/B01NARJJXT", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Puella Magi Madoka Magica - Part 2 (English Dubbed)", "https://www.amazon.com/Theres-Way-Ill-Ever-Regret/dp/B00EHAM8SQ", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Space Dandy (Original Japanese Version)", "https://www.amazon.com/Cant-Be-Only-One-Baby/dp/B00NAKJ9CM", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Soul Eater (Original Japanese Version)", "https://www.amazon.com/800-Years-Bloodlust-Advent-Heretic/dp/B01MRCFR21", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Season 2 - Legend of Kyoto", "https://www.amazon.com/Devil-Vengeance-Makoto-Shishios-Plot/dp/B06XNYR5T8", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Season 3 - Tales of the Meiji", "https://www.amazon.com/Legend-Fireflies-Girl-Waits-Love/dp/B06XL17BN9", StreamingService.AmazonAnimeStrike)
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
