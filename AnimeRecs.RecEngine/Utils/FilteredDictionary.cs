using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimeRecs.RecEngine.Utils
{
    /// <summary>
    /// A dictionary backed by another dictionary where some of the entries in the backing dictionary should be filtered out.
    /// This class is optimized for space-efficient storage in the case where most of the entries in the backing dictionary are valid.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class FilteredDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> m_backingDictionary;
        private Predicate<KeyValuePair<TKey, TValue>> m_keyAllowedPredicate;
        private HashSet<TKey> m_invalidKeys;
        private FilteredDictionaryKeyCollection m_keyCollection;
        private FilteredDictionaryValueCollection m_valueCollection;

        public FilteredDictionary(IDictionary<TKey, TValue> backingDictionary, Predicate<KeyValuePair<TKey, TValue>> keyAllowedPredicate)
        {
            m_backingDictionary = backingDictionary;
            m_keyAllowedPredicate = keyAllowedPredicate;
            m_invalidKeys = new HashSet<TKey>(backingDictionary.Where(kvPair => !m_keyAllowedPredicate(kvPair)).Select(kvPair => kvPair.Key));
            m_keyCollection = new FilteredDictionaryKeyCollection(this);
            m_valueCollection = new FilteredDictionaryValueCollection(this);
        }

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            return m_backingDictionary.ContainsKey(key) && !m_invalidKeys.Contains(key);
        }

        public ICollection<TKey> Keys
        {
            get { return m_keyCollection; }
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (m_invalidKeys.Contains(key))
            {
                value = default(TValue);
                return false;
            }
            else
            {
                return m_backingDictionary.TryGetValue(key, out value);
            }
        }

        public ICollection<TValue> Values
        {
            get { return m_valueCollection; }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (m_invalidKeys.Contains(key))
                {
                    throw new KeyNotFoundException();
                }
                else
                {
                    return m_backingDictionary[key];
                }
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            if (m_invalidKeys.Contains(item.Key))
            {
                return false;
            }
            else
            {
                return m_backingDictionary.Contains(item);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return m_backingDictionary.Count - m_invalidKeys.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return m_backingDictionary.Where(kvPair => !m_invalidKeys.Contains(kvPair.Key)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (System.Collections.IEnumerator)GetEnumerator();
        }

        private class FilteredDictionaryKeyCollection : ICollection<TKey>
        {
            private FilteredDictionary<TKey, TValue> m_filteredDict;

            public FilteredDictionaryKeyCollection(FilteredDictionary<TKey, TValue> filteredDict)
            {
                m_filteredDict = filteredDict;
            }
            
            public void Add(TKey item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(TKey item)
            {
                return !m_filteredDict.m_invalidKeys.Contains(item) && m_filteredDict.ContainsKey(item);
            }

            public void CopyTo(TKey[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return m_filteredDict.Count - m_filteredDict.m_invalidKeys.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public bool Remove(TKey item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<TKey> GetEnumerator()
            {
                return m_filteredDict.m_backingDictionary.Keys.Where(key => !m_filteredDict.m_invalidKeys.Contains(key)).GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return (System.Collections.IEnumerator)GetEnumerator();
            }
        }

        private class FilteredDictionaryValueCollection : ICollection<TValue>
        {
            private FilteredDictionary<TKey, TValue> m_filteredDict;

            public FilteredDictionaryValueCollection(FilteredDictionary<TKey, TValue> filteredDict)
            {
                m_filteredDict = filteredDict;
            }
            
            public void Add(TValue item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(TValue item)
            {
                return m_filteredDict.Any(kvPair => !m_filteredDict.m_invalidKeys.Contains(kvPair.Key) && object.Equals(kvPair.Value, item));
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return m_filteredDict.m_backingDictionary.Count - m_filteredDict.m_invalidKeys.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public bool Remove(TValue item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<TValue> GetEnumerator()
            {
                return m_filteredDict.m_backingDictionary
                    .Where(kvPair => !m_filteredDict.m_invalidKeys.Contains(kvPair.Key))
                    .Select(kvPair => kvPair.Value)
                    .GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return (System.Collections.IEnumerator)GetEnumerator();
            }
        }
    }
}
