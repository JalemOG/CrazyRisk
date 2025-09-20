using System;

namespace CrazyRisk.DataStructures
{
    public class HashMap<K, V>
    {
        private class Entry
        {
            public K Key { get; set; }
            public V Value { get; set; }
            
            public Entry(K key, V value)
            {
                Key = key;
                Value = value;
            }
            
            public override bool Equals(object obj)
            {
                if (obj is Entry other)
                    return Key.Equals(other.Key);
                return false;
            }
            
            public override int GetHashCode()
            {
                return Key.GetHashCode();
            }
        }
        
        private LinkedList<Entry>[] buckets;
        private int capacity;
        private int size;
        
        public HashMap(int capacity = 16)
        {
            this.capacity = capacity;
            buckets = new LinkedList<Entry>[capacity];
            size = 0;
        }
        
        public int Size { get { return size; } }
        
        private int GetBucketIndex(K key)
        {
            int hashCode = key.GetHashCode();
            return Math.Abs(hashCode) % capacity;
        }
        
        public void Put(K key, V value)
        {
            int bucketIndex = GetBucketIndex(key);
            
            if (buckets[bucketIndex] == null)
                buckets[bucketIndex] = new LinkedList<Entry>();
                
            LinkedList<Entry> bucket = buckets[bucketIndex];
            Entry newEntry = new Entry(key, value);
            Node<Entry> existing = bucket.Find(newEntry);
            
            if (existing != null)
            {
                existing.Value.Value = value;
            }
            else
            {
                bucket.Add(newEntry);
                size++;
            }
        }
        
        public V Get(K key)
        {
            int bucketIndex = GetBucketIndex(key);
            
            if (buckets[bucketIndex] == null)
                return default(V);
                
            LinkedList<Entry> bucket = buckets[bucketIndex];
            Entry searchEntry = new Entry(key, default(V));
            Node<Entry> result = bucket.Find(searchEntry);
            
            return result != null ? result.Value.Value : default(V);
        }
        
        public bool ContainsKey(K key)
        {
            return Get(key) != null;
        }
        
        public bool Remove(K key)
        {
            int bucketIndex = GetBucketIndex(key);
            
            if (buckets[bucketIndex] == null)
                return false;
                
            LinkedList<Entry> bucket = buckets[bucketIndex];
            Entry entryToRemove = new Entry(key, default(V));
            
            bool removed = bucket.Remove(entryToRemove);
            if (removed)
                size--;
                
            return removed;
        }
        
        public LinkedList<K> Keys()
        {
            LinkedList<K> keys = new LinkedList<K>();
            
            for (int i = 0; i < capacity; i++)
            {
                if (buckets[i] != null)
                {
                    IIterator<Entry> iterator = buckets[i].GetIterator();
                    while (iterator.HasNext())
                    {
                        keys.Add(iterator.Next().Key);
                    }
                }
            }
            
            return keys;
        }
    }
}