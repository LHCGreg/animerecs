using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AnimeRecs.DAL;
using Moq;
using Xunit;

namespace AnimeRecs.UpdateStreams.UnitTests
{
    public class HidiveStreamInfoSourceTests
    {
        [Fact]
        public void TestHelperStreamInfoSource()
        {
            IWebClient mockWebClient = GetMockWebClient();
            HidiveStreamInfoSource.HelperStreamInfoSource source = new HidiveStreamInfoSource.HelperStreamInfoSource("https://www.hidive.com/tv", mockWebClient);
            ICollection<AnimeStreamInfo> streams = source.GetAnimeStreamInfoAsync(CancellationToken.None).GetAwaiter().GetResult();
            AssertTvStreamsAreCorrect(streams);
        }

        [Fact]
        public void TestHelperStreamInfoSourceOnEmpty()
        {
            IWebClient mockWebClient = GetMockWebClient();
            HidiveStreamInfoSource.HelperStreamInfoSource source = new HidiveStreamInfoSource.HelperStreamInfoSource("https://www.hidive.com/empty", mockWebClient);
            Assert.Throws<NoMatchingHtmlException>(() => source.GetAnimeStreamInfoAsync(CancellationToken.None).GetAwaiter().GetResult());
        }

        [Fact]
        public void TestGetAnimeStreamInfo()
        {
            IWebClient mockWebClient = GetMockWebClient();
            HidiveStreamInfoSource source = new HidiveStreamInfoSource(mockWebClient);
            ICollection<AnimeStreamInfo> streams = source.GetAnimeStreamInfoAsync(CancellationToken.None).GetAwaiter().GetResult();
            AssertCombinedStreamsAreCorrect(streams);
        }

        private static IWebClient GetMockWebClient()
        {
            Dictionary<Uri, IWebClientResult> mockResultsByURL = new Dictionary<Uri, IWebClientResult>();

            StringWebClientResult tvResult = new StringWebClientResult(Helpers.GetResourceText("hidive_tv.html"));
            mockResultsByURL[new Uri("https://www.hidive.com/tv")] = tvResult;

            StringWebClientResult moviesResult = new StringWebClientResult(Helpers.GetResourceText("hidive_movies.html"));
            mockResultsByURL[new Uri("https://www.hidive.com/movies")] = moviesResult;

            StringWebClientResult emptyResult = new StringWebClientResult(Helpers.GetResourceText("hidive_empty.html"));
            mockResultsByURL[new Uri("https://www.hidive.com/empty")] = emptyResult;

            var webClientMock = new Mock<IWebClient>();
            webClientMock.Setup(client => client.GetAsync(It.IsAny<WebClientRequest>(), It.IsAny<CancellationToken>()))
                .Returns<WebClientRequest, CancellationToken>((request, token) => Task.FromResult(mockResultsByURL[request.URL]));
            return webClientMock.Object;
        }

        private static void AssertTvStreamsAreCorrect(ICollection<AnimeStreamInfo> tvStreams)
        {
            // correct count
            Assert.Equal(365, tvStreams.Count);

            // all have correct source
            Assert.All(tvStreams, stream => Assert.Equal(StreamingService.Hidive, stream.Service));

            AssertContainsTvStreams(tvStreams);
        }

        private static void AssertCombinedStreamsAreCorrect(ICollection<AnimeStreamInfo> allStreams)
        {
            // correct count
            Assert.Equal(475, allStreams.Count);

            // all have correct source
            Assert.All(allStreams, stream => Assert.Equal(StreamingService.Hidive, stream.Service));

            AssertContainsTvStreams(allStreams);
            AssertContainsMovieStreams(allStreams);
        }

        private static void AssertContainsTvStreams(ICollection<AnimeStreamInfo> streams)
        {
            // Check first, last, and some in between
            AnimeStreamInfo firstStream = new AnimeStreamInfo("A Little Snow Fairy Sugar",
                "https://www.hidive.com/stream/a-little-snow-fairy-sugar/s01e001", StreamingService.Hidive);
            Assert.Contains(firstStream, streams);

            AnimeStreamInfo lastStream = new AnimeStreamInfo("Yuyushiki",
                "https://www.hidive.com/stream/yuyushiki/s01e001", StreamingService.Hidive);
            Assert.Contains(lastStream, streams);

            AnimeStreamInfo middleStream1 = new AnimeStreamInfo("Hayate the Combat Butler! Can't Take My Eyes Off You",
                "https://www.hidive.com/stream/hayate-the-combat-butler-cant-take-my-eyes-off-you/s03e001", StreamingService.Hidive);
            Assert.Contains(middleStream1, streams);

            AnimeStreamInfo middleStream2 = new AnimeStreamInfo("The Familiar of Zero: \"Rondo\" of Princesses",
                "https://www.hidive.com/stream/the-familiar-of-zero-rondo-of-princesses/s03e001", StreamingService.Hidive);
            Assert.Contains(middleStream2, streams);

            AnimeStreamInfo middleStream3 = new AnimeStreamInfo("Dog & Scissors",
                "https://www.hidive.com/stream/dog-scissors/s01e001", StreamingService.Hidive);
            Assert.Contains(middleStream3, streams);

            AnimeStreamInfo middleStream4 = new AnimeStreamInfo("Croisée in a Foreign Labyrinth",
                "https://www.hidive.com/stream/croisee-in-a-foreign-labyrinth/s01e001", StreamingService.Hidive);
            Assert.Contains(middleStream4, streams);
        }

        private static void AssertContainsMovieStreams(ICollection<AnimeStreamInfo> streams)
        {
            // Check first, last, and some in between
            AnimeStreamInfo firstStream = new AnimeStreamInfo("Amagi Brilliant Park OVA",
                "https://www.hidive.com/stream/amagi-brilliant-park-ova/2015062614", StreamingService.Hidive);
            Assert.Contains(firstStream, streams);

            AnimeStreamInfo lastStream = new AnimeStreamInfo("The World God Only Knows OVA",
                "https://www.hidive.com/stream/the-world-god-only-knows-ova/2011091601", StreamingService.Hidive);
            Assert.Contains(lastStream, streams);

            AnimeStreamInfo middleStream1 = new AnimeStreamInfo("Rozen Maiden: Ouvertüre",
                "https://www.hidive.com/stream/rozen-maiden-ouverture/2006122201", StreamingService.Hidive);
            Assert.Contains(middleStream1, streams);

            AnimeStreamInfo middleStream2 = new AnimeStreamInfo("Love, Chunibyo & Other Delusions! OVA",
                "https://www.hidive.com/stream/love-chunibyo-other-delusions-ova/2013061913", StreamingService.Hidive);
            Assert.Contains(middleStream2, streams);
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
