using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.MAL
{
    /// <summary>
    /// Memory-efficient read-only dictionary from anime id to MalListEntry implemented as a sorted List.
    /// </summary>
    public class ReadOnlyMalListEntryDictionary : IDictionary<int, MalListEntry>
    {
        private List<ListEntryAndAnimeId> m_entries;
        private ReadOnlyMalListEntryDictionaryKeyCollection m_keyCollection;
        private ReadOnlyMalListEntryDictionaryValueCollection m_valueCollection;

        /// <summary>
        /// An array of MAL list entries and their anime ids. This class will assume ownership of the List and sort it.
        /// </summary>
        /// <param name="entries"></param>
        public ReadOnlyMalListEntryDictionary(List<ListEntryAndAnimeId> entries)
        {
            entries.Sort((entryAndAnimeId1, entryAndAnimeId2) => entryAndAnimeId1.AnimeId.CompareTo(entryAndAnimeId2.AnimeId));
            m_entries = entries;
            m_keyCollection = new ReadOnlyMalListEntryDictionaryKeyCollection(this);
            m_valueCollection = new ReadOnlyMalListEntryDictionaryValueCollection(this);
        }

        public struct ListEntryAndAnimeId
        {
            private readonly int m_animeId;
            public int AnimeId { get { return m_animeId; } }

            private readonly MalListEntry m_entry;
            public MalListEntry Entry { get { return m_entry; } }

            public ListEntryAndAnimeId(int animeId, MalListEntry entry)
            {
                m_animeId = animeId;
                m_entry = entry;
            }

            public override string ToString()
            {
                return AnimeId.ToString();
            }
        }

        private int? GetIndexOfAnimeId(int animeId)
        {
            // Travel back in time to your data structures class and recall the binary search.

            int lowBound = 0;
            int highBound = m_entries.Count - 1;
            while (lowBound <= highBound)
            {
                int guessId = (lowBound + highBound) / 2;

                if (m_entries[guessId].AnimeId == animeId)
                {
                    // Found it!
                    return guessId;
                }
                else if (m_entries[guessId].AnimeId > animeId)
                {
                    // Guessed too high
                    highBound = guessId - 1;
                }
                else
                {
                    // Guessed too low
                    lowBound = guessId + 1;
                }
            }

            // Didn't find it
            return null;
        }

        public void Add(int key, MalListEntry value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(int key)
        {
            return GetIndexOfAnimeId(key) != null;
        }

        public ICollection<int> Keys { get { return m_keyCollection; } }

        public bool Remove(int key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(int key, out MalListEntry value)
        {
            int? index = GetIndexOfAnimeId(key);
            if (index == null)
            {
                value = default(MalListEntry);
                return false;
            }
            else
            {
                value = m_entries[index.Value].Entry;
                return true;
            }
        }

        public ICollection<MalListEntry> Values { get { return m_valueCollection; } }

        public MalListEntry this[int key]
        {
            get
            {
                int? index = GetIndexOfAnimeId(key);
                if (index == null)
                {
                    throw new KeyNotFoundException(string.Format("No anime with id {0}.", key));
                }
                else
                {
                    return m_entries[index.Value].Entry;
                }
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<int, MalListEntry> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<int, MalListEntry> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<int, MalListEntry>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count { get { return m_entries.Count; } }

        public bool IsReadOnly { get { return true; } }

        public bool Remove(KeyValuePair<int, MalListEntry> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<int, MalListEntry>> GetEnumerator()
        {
            foreach (ListEntryAndAnimeId entryAndAnimeId in m_entries)
            {
                yield return new KeyValuePair<int, MalListEntry>(entryAndAnimeId.AnimeId, entryAndAnimeId.Entry);
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }





        private class ReadOnlyMalListEntryDictionaryKeyCollection : ICollection<int>
        {
            private readonly ReadOnlyMalListEntryDictionary m_dict;

            public ReadOnlyMalListEntryDictionaryKeyCollection(ReadOnlyMalListEntryDictionary dict)
            {
                m_dict = dict;
            }

            public void Add(int item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(int item)
            {
                return m_dict.GetIndexOfAnimeId(item) != null;
            }

            public void CopyTo(int[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count { get { return m_dict.m_entries.Count; } }

            public bool IsReadOnly { get { return true; } }

            public bool Remove(int item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<int> GetEnumerator()
            {
                foreach (ListEntryAndAnimeId entryAndAnimeId in m_dict.m_entries)
                {
                    yield return entryAndAnimeId.AnimeId;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class ReadOnlyMalListEntryDictionaryValueCollection : ICollection<MalListEntry>
        {
            private readonly ReadOnlyMalListEntryDictionary m_dict;

            public ReadOnlyMalListEntryDictionaryValueCollection(ReadOnlyMalListEntryDictionary dict)
            {
                m_dict = dict;
            }

            public void Add(MalListEntry item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(MalListEntry item)
            {
                return m_dict.m_entries.Any(entryAndAnimeId => entryAndAnimeId.Entry.Equals(item));
            }

            public void CopyTo(MalListEntry[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count { get { return m_dict.m_entries.Count; } }

            public bool IsReadOnly { get { return true; } }

            public bool Remove(MalListEntry item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<MalListEntry> GetEnumerator()
            {
                foreach (ListEntryAndAnimeId entryAndAnimeId in m_dict.m_entries)
                {
                    yield return entryAndAnimeId.Entry;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}

// Copyright (C) 2012 Greg Najda
//
// This file is part of AnimeRecs.RecEngine.MAL.
//
// AnimeRecs.RecEngine.MAL is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// AnimeRecs.RecEngine.MAL is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with AnimeRecs.RecEngine.MAL.  If not, see <http://www.gnu.org/licenses/>.