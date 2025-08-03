using System.Collections;

namespace Diglett.Core.Collections
{
    public class Multimap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, ICollection<TValue>>>
        where TKey : notnull
    {
        private readonly IDictionary<TKey, ICollection<TValue>> _dict;
        private readonly Func<IEnumerable<TValue>, ICollection<TValue>> _collectionCreator;
        private readonly bool _isReadonly = false;

        internal readonly static Func<IEnumerable<TValue>, ICollection<TValue>> DefaultCollectionCreator =
            x => new List<TValue>(x ?? []);

        public Multimap()
            : this(EqualityComparer<TKey>.Default) { }

        public Multimap(IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, ICollection<TValue>>(comparer ?? EqualityComparer<TKey>.Default);
            _collectionCreator = DefaultCollectionCreator;
        }

        public Multimap(Func<IEnumerable<TValue>, ICollection<TValue>> collectionCreator)
            : this(new Dictionary<TKey, ICollection<TValue>>(), collectionCreator) { }

        public Multimap(IEqualityComparer<TKey> comparer, Func<IEnumerable<TValue>, ICollection<TValue>> collectionCreator)
            : this(new Dictionary<TKey, ICollection<TValue>>(comparer ?? EqualityComparer<TKey>.Default), collectionCreator) { }

        internal Multimap(IDictionary<TKey, ICollection<TValue>> dictionary, Func<IEnumerable<TValue>, ICollection<TValue>> collectionCreator)
        {
            Guard.NotNull(dictionary);
            Guard.NotNull(collectionCreator);

            _dict = dictionary;
            _collectionCreator = collectionCreator;
        }

        protected virtual ICollection<TValue> CreateCollection(IEnumerable<TValue>? values)
        {
            return (_collectionCreator ?? DefaultCollectionCreator)(values ?? []);
        }

        public int Count => _dict.Keys.Count;
        public int TotalValueCount => _dict.Values.Sum(x => x.Count);

		public virtual ICollection<TValue>? this[TKey key]
        {
            get
            {
                if (_dict.TryGetValue(key, out var values))
                {
                    return values;
                }

                if (!_isReadonly)
                {
                    values = CreateCollection(null);
                    _dict[key] = values;
                    return values;
                }

                return null;
            }
        }

        public virtual ICollection<TKey> Keys => _dict.Keys;
        public virtual ICollection<ICollection<TValue>> Values => _dict.Values;

        public IEnumerable<TValue> Find(TKey key, Func<TValue, bool> predicate)
        {
            Guard.NotNull(key);
            Guard.NotNull(predicate);

            if (_dict.TryGetValue(key, out var values))
            {
                return values.Where(predicate);
            }

            return [];
        }

        public virtual void Add(TKey key, TValue value)
        {
            CheckNotReadonly();

            this[key]?.Add(value);
        }

        public virtual void AddRange(TKey key, IEnumerable<TValue> values)
        {
            if (values.IsNullOrEmpty())
            {
                return;
            }

            CheckNotReadonly();

            this[key]?.AddRange(values);
        }

        public virtual bool Remove(TKey key, TValue value)
        {
            CheckNotReadonly();

            if (_dict.TryGetValue(key, out var values))
            {
                var removed = values.Remove(value);
                if (removed && values.Count == 0)
                {
                    _dict.Remove(key);
                }

                return removed;
            }

            return false;
        }

        public virtual bool RemoveAll(TKey key)
        {
            CheckNotReadonly();
            return _dict.Remove(key);
        }

        public virtual void Clear()
        {
            CheckNotReadonly();
            _dict.Clear();
        }

        public virtual bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public virtual bool TryGetValues(TKey key, out ICollection<TValue>? values)
        {
            return _dict.TryGetValue(key, out values);
        }

        public virtual bool ContainsValue(TKey key, TValue value)
        {
            return _dict.TryGetValue(key, out var values) && values.Contains(value);
        }

        public IDictionary<TKey, ICollection<TValue>> AsDictionary() => _dict;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

		public virtual IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        private void CheckNotReadonly()
        {
            if (_isReadonly)
                throw new NotSupportedException("Multimap is read-only.");
        }

        public static Multimap<TKey, TValue> CreateFromLookup(ILookup<TKey, TValue> source)
        {
            Guard.NotNull(source);

            var map = new Multimap<TKey, TValue>();

            foreach (IGrouping<TKey, TValue> group in source)
            {
                map.AddRange(group.Key, group);
            }

            return map;
        }
    }
}
