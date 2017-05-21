using AnimeRecs.DAL;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnimeRecs.UpdateStreams.Tests
{
    [TestFixture]
    public class AmazonAnimeStrikeStreamInfoSourceTests
    {
        [Test]
        public void TestGetAnimeStreamInfo()
        {
            IWebClient mockWebClient = GetMockWebClient();
            AmazonAnimeStrikeStreamInfoSource source = new AmazonAnimeStrikeStreamInfoSource(mockWebClient);
            ICollection<AnimeStreamInfo> streams = source.GetAnimeStreamInfo();
            Assert.That(streams, Is.EquivalentTo(ExpectedStreamInfo));
        }

        private static IWebClient GetMockWebClient()
        {
            var webClientMock = new Mock<IWebClient>();

            Dictionary<string, string> resourceNamesOfMockPages = new Dictionary<string, string>();
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_nr_p_n_subscription_id_1?rh=n:2858778011,p_n_ways_to_watch:12007866011,p_n_subscription_id:16182082011&ie=UTF8"] = "amazon_anime_strike_1.html";
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_pg_2?rh=n:2858778011%2Cp_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011&page=2&ie=UTF8&qid=1495396159"] = "amazon_anime_strike_2.html";
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_pg_9?rh=n:2858778011%2Cp_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011&page=9&ie=UTF8&qid=1495396218"] = "amazon_anime_strike_3.html";

            foreach (string url in resourceNamesOfMockPages.Keys)
            {
                string urlCopy = url;
                webClientMock.Setup(client => client.Get(urlCopy)).Returns(() => new WebClientResult(GetResourceReader(resourceNamesOfMockPages[urlCopy])));
            }

            return webClientMock.Object;
        }

        private static TextReader GetResourceReader(string name)
        {
            string resourceName = string.Format("AnimeRecs.UpdateStreams.Tests.{0}", name);
            return new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName), Encoding.UTF8);
        }

        private static List<AnimeStreamInfo> ExpectedStreamInfo = new List<AnimeStreamInfo>()
        {
            new AnimeStreamInfo("Armed Girl's Machiavellism - Season 1", "https://www.amazon.com/The-Dubious-Sword-Satori-Tamaba/dp/B06Y1Q4SM7/ref=sr_1_1?s=instant-video&ie=UTF8&sr=1-1&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Grimoire of Zero - Season 1", "https://www.amazon.com/Thirteen/dp/B06Y3CSGF1/ref=sr_1_2?s=instant-video&ie=UTF8&sr=1-2&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Re:CREATORS", "https://www.amazon.com/You-knows-where-justice-lies/dp/B06Y3WYQHG/ref=sr_1_3?s=instant-video&ie=UTF8&sr=1-3&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Sword Oratoria: Is it Wrong to Try to Pick Up Girls in a Dungeon? On the Side - Season 1", "https://www.amazon.com/Red-head-Lone-Ruler/dp/B071QY12G4/ref=sr_1_4?s=instant-video&ie=UTF8&sr=1-4&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("RAGE OF BAHAMUT VIRGIN SOUL", "https://www.amazon.com/Anatean-Holiday/dp/B06Y3LMLGW/ref=sr_1_5?s=instant-video&ie=UTF8&sr=1-5&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Anonymous Noise - Season 1", "https://www.amazon.com/keep-Walking-Forward-Today-Tomorrow/dp/B06Y5SCYK4/ref=sr_1_6?s=instant-video&ie=UTF8&sr=1-6&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Saekano- How to Raise a Boring Girlfriend .flat", "https://www.amazon.com/Episode5-Deadline-or-Awakening/dp/B06Y1X4HL1/ref=sr_1_7?s=instant-video&ie=UTF8&sr=1-7&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Akira (English Dubbed)", "https://www.amazon.com/Akira-English-Dubbed-Katsuhiro-Ohtomo/dp/B00G0AHDKE/ref=sr_1_8?s=instant-video&ie=UTF8&sr=1-8&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Sagrada Reset - Season 1", "https://www.amazon.com/WITCH-PICTURE-RED-EYE-GIRL/dp/B06Y1R8D86/ref=sr_1_9?s=instant-video&ie=UTF8&sr=1-9&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Paprika", "https://www.amazon.com/Paprika-Megumi-Hayashibara/dp/B00CX8O0NU/ref=sr_1_10?s=instant-video&ie=UTF8&sr=1-10&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("GRANBLUE FANTASY The Animation", "https://www.amazon.com/The-Iron-Giant/dp/B01MYASHGC/ref=sr_1_11?s=instant-video&ie=UTF8&sr=1-11&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Darker Than Black Season 1 (English Dubbed)", "https://www.amazon.com/Fallen-Star-Contract-Part/dp/B002S45X62/ref=sr_1_12?s=instant-video&ie=UTF8&sr=1-12&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Eromanga Sensei - Season 1", "https://www.amazon.com/Masamune-Izumi-Nemesis-Million-Copies/dp/B06Y4CWQJB/ref=sr_1_13?s=instant-video&ie=UTF8&sr=1-13&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Fullmetal Alchemist Brotherhood (English Dubbed) Season 1", "https://www.amazon.com/Fullmetal-Alchemist/dp/B003E3XIDQ/ref=sr_1_14?s=instant-video&ie=UTF8&sr=1-14&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Is It Wrong to Try to Pick Up Girls In a Dungeon? Season 1", "https://www.amazon.com/Bell-Cranel-ADVENTURER/dp/B01N0YCDM9/ref=sr_1_15?s=instant-video&ie=UTF8&sr=1-15&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Code Geass: Lelouch of the Rebellion, Season 1 (Original Japanese Version)", "https://www.amazon.com/Stage-01-Day-Demon-Born/dp/B06Y4FPL4X/ref=sr_1_16?s=instant-video&ie=UTF8&sr=1-16&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Atom: the Beginning - Season 1", "https://www.amazon.com/Step-on-it-Maruhige-Shipping/dp/B071Y1GY5P/ref=sr_1_17?s=instant-video&ie=UTF8&sr=1-17&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("One-Punch Man- Season 1", "https://www.amazon.com/The-Strongest-Man/dp/B01N4N4K4F/ref=sr_1_18?s=instant-video&ie=UTF8&sr=1-18&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("High School of the Dead Season 1 (English Dubbed)", "https://www.amazon.com/Spring-of-the-DEAD/dp/B06X92DXYF/ref=sr_1_19?s=instant-video&ie=UTF8&sr=1-19&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Scum's Wish", "https://www.amazon.com/Scums-Wish/dp/B01NCUV2FV/ref=sr_1_20?s=instant-video&ie=UTF8&sr=1-20&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Parasyte - the maxim - Season 1 (English Dubbed)", "https://www.amazon.com/The-Metamorphosis/dp/B0166I3GO6/ref=sr_1_21?s=instant-video&ie=UTF8&sr=1-21&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Black Bullet Season 1 (English Dubbed)", "https://www.amazon.com/The-Last-Hope/dp/B01NARYWVR/ref=sr_1_22?s=instant-video&ie=UTF8&sr=1-22&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Akame ga Kill! Season 1 (English Dubbed)", "https://www.amazon.com/Kill-the-Darkness/dp/B06VVNXLGT/ref=sr_1_23?s=instant-video&ie=UTF8&sr=1-23&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("No Game, No Life Season 1 (English Dubbed)", "https://www.amazon.com/Beginner/dp/B01NBTFPUH/ref=sr_1_24?s=instant-video&ie=UTF8&sr=1-24&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Darker Than Black", "https://www.amazon.com/Fallen-Star-Contract-Part/dp/B071H4RCZP/ref=sr_1_25?s=instant-video&ie=UTF8&sr=1-25&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Full Metal Panic! Season 1 (English Dubbed)", "https://www.amazon.com/Guy-Kinda-Like-Sergeant/dp/B0047C8LH6/ref=sr_1_26?s=instant-video&ie=UTF8&sr=1-26&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Blue Exorcist- Kyoto Saga", "https://www.amazon.com/Small-Beginnings/dp/B01N3384UP/ref=sr_1_27?s=instant-video&ie=UTF8&sr=1-27&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Vampire Hunter D", "https://www.amazon.com/Vampire-Hunter-D-Toyoo-Ashida/dp/B01N20N3LT/ref=sr_1_28?s=instant-video&ie=UTF8&sr=1-28&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Ergo Proxy Season 1 (English Dubbed)", "https://www.amazon.com/Awakening/dp/B002SEFWZY/ref=sr_1_29?s=instant-video&ie=UTF8&sr=1-29&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("ONIHEI", "https://www.amazon.com/Tanbei-of-Chigashira/dp/B01MR4S1XJ/ref=sr_1_30?s=instant-video&ie=UTF8&sr=1-30&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Soul Eater (English Dubbed)", "https://www.amazon.com/Resonance-Soul-Eater-Become-Scythe/dp/B0037OUWMW/ref=sr_1_31?s=instant-video&ie=UTF8&sr=1-31&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Maid-Sama! Season 1 (English Dubbed)", "https://www.amazon.com/Misa-is-a-Maid-Sama/dp/B01MT7TT7L/ref=sr_1_32?s=instant-video&ie=UTF8&sr=1-32&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("K - Season 1", "https://www.amazon.com/Knight/dp/B01N4MDEX6/ref=sr_1_129?s=instant-video&ie=UTF8&sr=1-129&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Clannad: After Story", "https://www.amazon.com/Farewell-End-Summer/dp/B01N7QJKJC/ref=sr_1_130?s=instant-video&ie=UTF8&sr=1-130&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Puella Magi Madoka Magica Season 1", "https://www.amazon.com/First-Met-Her-Dream-Something/dp/B01NARJJXT/ref=sr_1_131?s=instant-video&ie=UTF8&sr=1-131&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Puella Magi Madoka Magica - Part 2 (English Dubbed)", "https://www.amazon.com/Theres-Way-Ill-Ever-Regret/dp/B00EHAM8SQ/ref=sr_1_132?s=instant-video&ie=UTF8&sr=1-132&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Space Dandy (Original Japanese Version)", "https://www.amazon.com/Cant-Be-Only-One-Baby/dp/B00NAKJ9CM/ref=sr_1_133?s=instant-video&ie=UTF8&sr=1-133&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Soul Eater (Original Japanese Version)", "https://www.amazon.com/800-Years-Bloodlust-Advent-Heretic/dp/B01MRCFR21/ref=sr_1_134?s=instant-video&ie=UTF8&sr=1-134&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Season 2 - Legend of Kyoto", "https://www.amazon.com/Devil-Vengeance-Makoto-Shishios-Plot/dp/B06XNYR5T8/ref=sr_1_135?s=instant-video&ie=UTF8&sr=1-135&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike),
            new AnimeStreamInfo("Season 3 - Tales of the Meiji", "https://www.amazon.com/Legend-Fireflies-Girl-Waits-Love/dp/B06XL17BN9/ref=sr_1_136?s=instant-video&ie=UTF8&sr=1-136&refinements=p_n_ways_to_watch:12007866011%2Cp_n_subscription_id:16182082011", StreamingService.AmazonAnimeStrike)
        };
    }
}

// Copyright (C) 2017 Greg Najda
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
//
//  If you modify AnimeRecs.UpdateStreams.Tests, or any covered work, by linking 
//  or combining it with HTML Agility Pack (or a modified version of that 
//  library), containing parts covered by the terms of the Microsoft Public 
//  License, the licensors of AnimeRecs.UpdateStreams.Tests grant you additional 
//  permission to convey the resulting work. Corresponding Source for a non-
//  source form of such a combination shall include the source code for the parts 
//  of HTML Agility Pack used as well as that of the covered work.