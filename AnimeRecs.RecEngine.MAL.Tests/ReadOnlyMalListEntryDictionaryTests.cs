﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using AnimeRecs.RecEngine.MAL;
using MalApi;

namespace AnimeRecs.RecEngine.MAL.Tests
{
    [TestFixture]
    public class ReadOnlyMalListEntryDictionaryTests
    {
        [Test]
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
            Assert.That(dict[3].NumEpisodesWatched, Is.EqualTo(3));
            Assert.That(dict[1].NumEpisodesWatched, Is.EqualTo(1));
            Assert.That(dict[9].NumEpisodesWatched, Is.EqualTo(9));
            Assert.That(dict[25].NumEpisodesWatched, Is.EqualTo(25));
            Assert.That(dict[2].NumEpisodesWatched, Is.EqualTo(2));
            Assert.That(dict[6].NumEpisodesWatched, Is.EqualTo(6));
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.MAL.Tests.
//
// AnimeRecs.RecEngine.MAL.Tests is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine.MAL.Tests is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.MAL.Tests.  If not, see <http://www.gnu.org/licenses/>.