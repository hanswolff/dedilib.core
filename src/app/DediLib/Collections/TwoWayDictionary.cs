using System;
using System.Collections;
using System.Collections.Generic;

namespace DediLib.Collections
{
    /// <summary>
    /// Dictionary to encapsulate a 1:1 map for fast lookups in both directions
    /// </summary>
    public class TwoWayDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dict;
        private readonly Dictionary<TValue, TKey> _reverseDict;

        public TwoWayDictionary()
        {
            _dict = new Dictionary<TKey, TValue>();
            _reverseDict = new Dictionary<TValue, TKey>();
        }

        public TwoWayDictionary(IEqualityComparer<TKey> keyComparer)
        {
            _dict = new Dictionary<TKey, TValue>(keyComparer);
            _reverseDict = new Dictionary<TValue, TKey>();
        }

        public TwoWayDictionary(IEqualityComparer<TValue> valueComparer)
        {
            _dict = new Dictionary<TKey, TValue>();
            _reverseDict = new Dictionary<TValue, TKey>(valueComparer);
        }

        public TwoWayDictionary(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            _dict = new Dictionary<TKey, TValue>(keyComparer);
            _reverseDict = new Dictionary<TValue, TKey>(valueComparer);
        }

        public TwoWayDictionary(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException(nameof(dictionary));

            _dict = new Dictionary<TKey, TValue>(dictionary.Count * 4);
            _reverseDict = new Dictionary<TValue, TKey>(dictionary.Count * 4);
            foreach (var pair in dictionary)
                _reverseDict.Add(pair.Value, pair.Key);
        }

        public TwoWayDictionary(int capacity)
        {
            _dict = new Dictionary<TKey, TValue>(capacity);
            _reverseDict = new Dictionary<TValue, TKey>(capacity);
        }

        public TwoWayDictionary(int capacity, IEqualityComparer<TKey> keyComparer)
        {
            _dict = new Dictionary<TKey, TValue>(capacity, keyComparer);
            _reverseDict = new Dictionary<TValue, TKey>(capacity);
        }

        public TwoWayDictionary(int capacity, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            _dict = new Dictionary<TKey, TValue>(capacity, keyComparer);
            _reverseDict = new Dictionary<TValue, TKey>(capacity, valueComparer);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dict.Clear();
            _reverseDict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (!_dict.TryGetValue(item.Key, out value))
                return false;

            return Equals(item.Value, value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var pair in _dict)
            {
                array[arrayIndex++] = pair;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Contains(item) && Remove(item.Key);
        }

        public int Count => _dict.Count;

        public bool IsReadOnly => false;

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            _dict.Add(key, value);
            try
            {
                _reverseDict.Add(value, key);
            }
            catch
            {
                _dict.Remove(key);
                throw;
            }
        }

        public void Set(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (value == null) throw new ArgumentNullException(nameof(value));

            TValue previousValue;
            if (_dict.TryGetValue(key, out previousValue))
                _reverseDict.Remove(previousValue);

            _dict[key] = value;
            _reverseDict[value] = key;
        }

        public bool ContainsValue(TValue value)
        {
            return _reverseDict.ContainsKey(value);
        }

        public bool Remove(TKey key)
        {
            TValue value;
            if (!_dict.TryGetValue(key, out value)) return false;
            _dict.Remove(key);
            _reverseDict.Remove(value);
            return true;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            return _reverseDict.TryGetValue(value, out key);
        }

        public TKey GetKey(TValue value)
        {
            TKey key;
            if (!_reverseDict.TryGetValue(value, out key))
                throw new KeyNotFoundException();
            return key;
        }

        public TValue this[TKey key]
        {
            get { return _dict[key]; }
            set
            {
                TValue valLeft;
                if (!_dict.TryGetValue(key, out valLeft))
                {
                    Add(key, value);
                    return;
                }

                var removed = _reverseDict.Remove(valLeft);
                try
                {
                    _reverseDict.Add(value, key);
                }
                catch (ArgumentException ex)
                {
                    if (removed) _reverseDict.Add(valLeft, key);
                    throw new ArgumentException("New value is already set to a different key. Only unique 1:1 mappings are allowed.", ex);
                }
                _dict[key] = value;
            }
        }

        public ICollection<TKey> Keys => _dict.Keys;

        public ICollection<TValue> Values => _dict.Values;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
