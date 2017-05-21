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
    public class AmazonPrimeStreamInfoSourceTests
    {
        [Test]
        public void TestGetAnimeStreamInfo()
        {
            IWebClient mockWebClient = GetMockWebClient();
            AmazonPrimeStreamInfoSource source = new AmazonPrimeStreamInfoSource(mockWebClient);
            ICollection<AnimeStreamInfo> streams = source.GetAnimeStreamInfo();
            Assert.That(streams, Is.EquivalentTo(ExpectedStreamInfo));
        }

        private static IWebClient GetMockWebClient()
        {
            var webClientMock = new Mock<IWebClient>();

            Dictionary<string, string> resourceNamesOfMockPages = new Dictionary<string, string>();
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_rot_p_n_ways_to_watch_1?rh=n:2858778011,p_n_theme_browse-bin:2650364011,p_n_ways_to_watch:12007865011&ie=UTF8"] = "amazon_prime_1.html";
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_pg_2?rh=n:2858778011%2Cp_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011&page=2&ie=UTF8&qid=1495396353"] = "amazon_prime_2.html";
            resourceNamesOfMockPages["https://www.amazon.com/s/ref=sr_pg_6?rh=n:2858778011%2Cp_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011&page=6&ie=UTF8&qid=1495396393"] = "amazon_prime_3.html";

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
            new AnimeStreamInfo("The Legend of Korra Book 1", "https://www.amazon.com/A-Leaf-in-the-Wind/dp/B007QYRCX6/ref=sr_1_1?s=instant-video&ie=UTF8&sr=1-1&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("The Legend of Korra Book 3", "https://www.amazon.com/A-Breath-of-Fresh-Air/dp/B00LEXEPJ4/ref=sr_1_2?s=instant-video&ie=UTF8&sr=1-2&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("The Legend of Korra Book 2", "https://www.amazon.com/The-Southern-Lights/dp/B00F6PKYX2/ref=sr_1_3?s=instant-video&ie=UTF8&sr=1-3&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("ThunderCats (Original Series): The Complete First Season, Volume 1", "https://www.amazon.com/Exodus/dp/B005C8DB7E/ref=sr_1_4?s=instant-video&ie=UTF8&sr=1-4&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Legend Of the Millennium Dragon", "https://www.amazon.com/Legend-Millennium-Dragon-Kensho-Ono/dp/B06ZY384GY/ref=sr_1_5?s=instant-video&ie=UTF8&sr=1-5&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Mix Master: Final Force", "https://www.amazon.com/The-Mix-Master-is-Back/dp/B06XR5XD17/ref=sr_1_6?s=instant-video&ie=UTF8&sr=1-6&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Tenkai Knights - Rise of the Knights", "https://www.amazon.com/Tenkai-Knights-Rise-Brian-Beacock/dp/B00I8H72YY/ref=sr_1_7?s=instant-video&ie=UTF8&sr=1-7&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("KABANERI OF THE IRON FORTRESS(Subbed)", "https://www.amazon.com/Prayer-Offer/dp/B01DU4K5ES/ref=sr_1_8?s=instant-video&ie=UTF8&sr=1-8&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Elfen Lied - Season 01", "https://www.amazon.com/Elfen-Lied-01-Chance-Encounter/dp/B003B21SP0/ref=sr_1_9?s=instant-video&ie=UTF8&sr=1-9&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Broken Blade Season 1", "https://www.amazon.com/The-Time-of-Awakening/dp/B00AI91ODA/ref=sr_1_10?s=instant-video&ie=UTF8&sr=1-10&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Technotise: Edit and I", "https://www.amazon.com/Technotise-Edit-I-Sanda-Knezevic/dp/B06XRFPVQ5/ref=sr_1_11?s=instant-video&ie=UTF8&sr=1-11&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Robotech: The Complete Series - Digitally Remastered", "https://www.amazon.com/The-Macross-Saga-Boobytrap/dp/B01I4ICUAC/ref=sr_1_12?s=instant-video&ie=UTF8&sr=1-12&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("ThunderCats (Original Series): The Complete First Season, Volume 2", "https://www.amazon.com/Queen-of-8-Legs/dp/B005C8DDW2/ref=sr_1_13?s=instant-video&ie=UTF8&sr=1-13&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("UFO Ultramaiden", "https://www.amazon.com/UFO-Ultramaiden-01-Bathhouse-Angel/dp/B00425YJIS/ref=sr_1_14?s=instant-video&ie=UTF8&sr=1-14&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Mix Master: King of Cards", "https://www.amazon.com/Mix-Master-Cards-Season-Legend/dp/B00AVTPJ4C/ref=sr_1_15?s=instant-video&ie=UTF8&sr=1-15&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Get Backers", "https://www.amazon.com/Initials-Are-G-B/dp/B003E33S9A/ref=sr_1_16?s=instant-video&ie=UTF8&sr=1-16&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Demon King Daimao Season 1 (English Subtitled)", "https://www.amazon.com/The-Demon-King-Is-Born/dp/B00DAA5ZPW/ref=sr_1_17?s=instant-video&ie=UTF8&sr=1-17&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("ThunderCats (Original Series): The Complete Second Season, Volume 1", "https://www.amazon.com/ThunderCats-Ho-Part-1/dp/B005HGA9DA/ref=sr_1_18?s=instant-video&ie=UTF8&sr=1-18&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("RahXephon - Season 01", "https://www.amazon.com/RahXephon-01-Invasion-Capital/dp/B003B1VJ0U/ref=sr_1_19?s=instant-video&ie=UTF8&sr=1-19&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Little Snow Fairy Sugar", "https://www.amazon.com/Little-Snow-Fairy-Sugar-Meets/dp/B0040G8GTW/ref=sr_1_20?s=instant-video&ie=UTF8&sr=1-20&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("ThunderCats (Original Series): The Complete Second Season, Volume 2", "https://www.amazon.com/Exile-Isle/dp/B005HE8AWY/ref=sr_1_21?s=instant-video&ie=UTF8&sr=1-21&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Princess Tutu - Season 01", "https://www.amazon.com/Princess-Tutu-01-Duck-Prince/dp/B003B1VIXI/ref=sr_1_22?s=instant-video&ie=UTF8&sr=1-22&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Battle Disc", "https://www.amazon.com/Horrible-Golden-Family/dp/B01M0D2IDK/ref=sr_1_23?s=instant-video&ie=UTF8&sr=1-23&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Mythical Detective Loki Ragnarok", "https://www.amazon.com/Mythical-Detective-Loki-Ragnarok-01/dp/B004FJTWJM/ref=sr_1_24?s=instant-video&ie=UTF8&sr=1-24&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Delinquent Hamsters", "https://www.amazon.com/Delinquent-Hamsters/dp/B06XDWKFF6/ref=sr_1_25?s=instant-video&ie=UTF8&sr=1-25&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Amethyst - Princess of Gem World", "https://www.amazon.com/Village-of-the-Frogs/dp/B01J71JF1C/ref=sr_1_26?s=instant-video&ie=UTF8&sr=1-26&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Orphen Season 1", "https://www.amazon.com/Orphen-01-Sword-Baltanders/dp/B008P95CBA/ref=sr_1_27?s=instant-video&ie=UTF8&sr=1-27&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("The Mystical Laws", "https://www.amazon.com/Mystical-Laws-Isamu-Imakake/dp/B00TJZ2O2Y/ref=sr_1_28?s=instant-video&ie=UTF8&sr=1-28&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("UFO Ultramaiden Season 2", "https://www.amazon.com/UFO-Ultramaiden-01-Key-Time/dp/B0041WL02K/ref=sr_1_29?s=instant-video&ie=UTF8&sr=1-29&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Robotech: The Complete Series - Digitally Remastered", "https://www.amazon.com/The-Masters-Saga-Danas-Story/dp/B01I4F5L9M/ref=sr_1_30?s=instant-video&ie=UTF8&sr=1-30&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Robotech: The Shadow Chronicles", "https://www.amazon.com/Robotech-Shadow-Chronicles-Mark-Hamill/dp/B01HPMWXU0/ref=sr_1_31?s=instant-video&ie=UTF8&sr=1-31&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Swiss Family Robinson", "https://www.amazon.com/The-Adventure-Begins/dp/B019EFG1MC/ref=sr_1_32?s=instant-video&ie=UTF8&sr=1-32&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Clip: The travel of a commuter.", "https://www.amazon.com/Clip-travel-commuter-Shigeo-Fukushima/dp/B01MYRDR43/ref=sr_1_81?s=instant-video&ie=UTF8&sr=1-81&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime),
            new AnimeStreamInfo("Game-Con Haul! My Loot Bag!", "https://www.amazon.com/Game-Con-Haul-My-Loot-Bag/dp/B01HSTE5NI/ref=sr_1_82?s=instant-video&ie=UTF8&sr=1-82&refinements=p_n_theme_browse-bin:2650364011%2Cp_n_ways_to_watch:12007865011", StreamingService.AmazonPrime)
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