using System;

namespace CrazyRisk.DataStructures
{
    public class HashMap<K, V>
    {
        // Clase interna para las entradas del mapa
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
                return obj is Entry other && Key.Equals(other.Key);
            }

            public override int GetHashCode()
            {
                return Key.GetHashCode();
            }
        }

        private LinkedList<Entry>[] buckets;
        private int capacity;
        private int size;
        private const int DEFAULT_CAPACITY = 16;

        public HashMap(int capacity = DEFAULT_CAPACITY)
        {
            this.capacity = capacity;
            buckets = new LinkedList<Entry>[capacity];
            for (int i = 0; i < capacity; i++)
                buckets[i] = new LinkedList<Entry>();
            size = 0;
        }

        private int GetBucketIndex(K key)
        {
            return Math.Abs(key.GetHashCode()) % capacity;
        }

        // Agregar o actualizar un valor
        public void Put(K key, V value)
        {
            int index = GetBucketIndex(key);
            LinkedList<Entry> bucket = buckets[index];

            Entry existing = bucket.Find(e => e.Key.Equals(key));
            if (existing != null)
            {
                existing.Value = value; // Actualizar valor existente
            }
            else
            {
                bucket.Add(new Entry(key, value));
                size++;
            }
        }

        // Obtener un valor
        public V Get(K key)
        {
            int index = GetBucketIndex(key);
            LinkedList<Entry> bucket = buckets[index];

            Entry existing = bucket.Find(e => e.Key.Equals(key));
            return existing != null ? existing.Value : default(V);
        }

        // Verificar si contiene una clave
        public bool ContainsKey(K key)
        {
            int index = GetBucketIndex(key);
            LinkedList<Entry> bucket = buckets[index];
            return bucket.Find(e => e.Key.Equals(key)) != null;
        }

        // Remover una entrada
        public bool Remove(K key)
        {
            int index = GetBucketIndex(key);
            LinkedList<Entry> bucket = buckets[index];

            Entry existing = bucket.Find(e => e.Key.Equals(key));
            if (existing != null)
            {
                bucket.Remove(existing);
                size--;
                return true;
            }
            return false;
        }

        // Obtener todas las claves
        public LinkedList<K> Keys()
        {
            LinkedList<K> keys = new LinkedList<K>();
            foreach (var bucket in buckets)
            {
                IIterator<Entry> it = bucket.GetIterator();
                while (it.HasNext())
                    keys.Add(it.Next().Key);
            }
            return keys;
        }

        // Obtener todos los valores
        public LinkedList<V> Values()
        {
            LinkedList<V> values = new LinkedList<V>();
            foreach (var bucket in buckets)
            {
                IIterator<Entry> it = bucket.GetIterator();
                while (it.HasNext())
                    values.Add(it.Next().Value);
            }
            return values;
        }

        // Obtener tamaño
        public int Size() => size;

        // Verificar si está vacío
        public bool IsEmpty() => size == 0;

        // Limpiar el mapa
        public void Clear()
        {
            foreach (var bucket in buckets)
                bucket.Clear();
            size = 0;
        }
    }
}
