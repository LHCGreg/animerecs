using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using AnimeRecs.RecEngine.MAL;
using MalApi;

namespace AnimeRecs.RecEngine.MAL.UnitTests
{
    public class ReadOnlyMalListEntryDictionaryTests
    {
        [Fact]
        public void TestIndexing()
        {
            List<ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId> entries = new List<ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId>()
            {
                new ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId(3, new MalListEntry(1, CompletionStatus.Completed, 3)),
                new ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId(1, new MalListEntry(2, CompletionStatus.Completed, 1)),
                new ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId(9, new MalListEntry(3, CompletionStatus.Completed, 9)),
                new ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId(25, new MalListEntry(4, CompletionStatus.Completed, 25)),
                new ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId(2, new MalListEntry(5, CompletionStatus.Completed, 2)),
                new ReadOnlyMalListEntryDictionary.ListEntryAndAnimeId(6, new MalListEntry(6, CompletionStatus.Completed, 6))
            };

            ReadOnlyMalListEntryDictionary dict = new ReadOnlyMalListEntryDictionary(entries);
            Assert.Equal(3, dict[3].NumEpisodesWatched);
            Assert.Equal(1, dict[1].NumEpisodesWatched);
            Assert.Equal(9, dict[9].NumEpisodesWatched);
            Assert.Equal(25, dict[25].NumEpisodesWatched);
            Assert.Equal(2, dict[2].NumEpisodesWatched);
            Assert.Equal(6, dict[6].NumEpisodesWatched);
        }
    }
}
