using System;
using System.Collections.Generic;

namespace CrazyRisk.DataStructures
{
    public class HashMap<K, V>
    {
        private readonly LinkedList<Entry>[] buckets;
        private readonly int capacity;
        private int size;

        private class Entry
        {
            public K Key { get; set; }
            public V Value { get; set; }

            public Entry(K key, V value)
            {
                Key = key;
                Value = value;
            }

            public override bool Equals(object? obj)
            {
                return obj is Entry other &&
                       EqualityComparer<K>.Default.Equals(Key, other.Key);
            }

            public override int GetHashCode()
            {
                return EqualityComparer<K>.Default.GetHashCode(Key!);
            }
        }

        public HashMap(int capacity = 16)
        {
            if (capacity <= 0) capacity = 16;

            this.capacity = capacity;
            buckets = new LinkedList<Entry>[capacity];
            for (int i = 0; i < capacity; i++)
                buckets[i] = new LinkedList<Entry>();
        }

        private int Index(K key)
        {
            int h = EqualityComparer<K>.Default.GetHashCode(key!);
            if (h < 0) h = -h;
            return h % capacity;
        }

        // ⬇⬇⬇  Put NO devuelve nada; elimina cualquier return algo; que tuvieras
        public void Put(K key, V value)
        {
            int idx = Index(key);
            var bucket = buckets[idx];

            var existing = bucket.Find(e => EqualityComparer<K>.Default.Equals(e.Key, key));
            if (existing is not null)
            {
                existing.Value = value;
                return;
            }

            bucket.Add(new Entry(key, value));
            size++;
        }

        public bool TryGet(K key, out V value)
        {
            int idx = Index(key);
            var bucket = buckets[idx];
            var entry = bucket.Find(e => EqualityComparer<K>.Default.Equals(e.Key, key));

            if (entry is not null)
            {
                value = entry.Value;
                return true;
            }

            value = default!;
            return false;
        }

        public V Get(K key)
        {
            if (TryGet(key, out var v))
                return v;
            throw new KeyNotFoundException($"Key not found: {key}");
        }

        public bool ContainsKey(K key)
        {
            int idx = Index(key);
            var bucket = buckets[idx];
            return bucket.Find(e => EqualityComparer<K>.Default.Equals(e.Key, key)) is not null;
        }

        public bool Remove(K key)
        {
            int idx = Index(key);
            var bucket = buckets[idx];

            var entry = bucket.Find(e => EqualityComparer<K>.Default.Equals(e.Key, key));
            if (entry is null) return false;

            bucket.Remove(entry);
            size--;
            return true;
        }

        public int Count => size;

        public LinkedList<K> Keys()
        {
            var keys = new LinkedList<K>();
            foreach (var bucket in buckets)
            {
                var it = bucket.GetIterator();
                while (it.HasNext())
                {
                    var e = it.Next();
                    keys.Add(e.Key);
                }
            }
            return keys;
        }

        public LinkedList<V> Values()
        {
            var values = new LinkedList<V>();
            foreach (var bucket in buckets)
            {
                var it = bucket.GetIterator();
                while (it.HasNext())
                {
                    var e = it.Next();
                    values.Add(e.Value);
                }
            }
            return values;
        }

        public void Clear()
        {
            foreach (var bucket in buckets)
                bucket.Clear();
            size = 0;
        }
    }
}