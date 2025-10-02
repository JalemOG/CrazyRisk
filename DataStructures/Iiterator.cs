namespace CrazyRisk.DataStructures
{
    // ============================================
    // INTERFAZ PRINCIPAL DEL ITERADOR
    // ============================================
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }
    
    // ============================================
    // ITERADOR PARA LINKEDLIST
    // ============================================
    public class LinkedListIterator<T> : IIterator<T>
    {
        private Node<T> current;
        
        public LinkedListIterator(Node<T> head)
        {
            current = head;
        }
        
        public bool HasNext()
        {
            return current != null;
        }
        
        public T Next()
        {
            if (!HasNext())
                throw new System.InvalidOperationException("No hay más elementos en la lista");
                
            T data = current.Data;
            current = current.Next;
            return data;
        }
    }
    
    // ============================================
    // NODO PARA LINKEDLIST
    // ============================================
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }
        
        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }
    
    // ============================================
    // LINKEDLIST IMPLEMENTACIÓN
    // ============================================
    public class LinkedList<T>
    {
        private Node<T> head;
        private Node<T> tail;
        private int size;
        
        public LinkedList()
        {
            head = null;
            tail = null;
            size = 0;
        }
        
        // Agregar al final de la lista
        public void Add(T data)
        {
            Node<T> newNode = new Node<T>(data);
            
            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
            }
            
            size++;
        }
        
        // Agregar al inicio de la lista
        public void AddFirst(T data)
        {
            Node<T> newNode = new Node<T>(data);
            
            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                newNode.Next = head;
                head = newNode;
            }
            
            size++;
        }
        
        // Remover un elemento específico
        public bool Remove(T data)
        {
            if (head == null)
                return false;
            
            // Si es el primer elemento
            if (head.Data.Equals(data))
            {
                head = head.Next;
                if (head == null)
                    tail = null;
                size--;
                return true;
            }
            
            // Buscar en el resto de la lista
            Node<T> current = head;
            while (current.Next != null)
            {
                if (current.Next.Data.Equals(data))
                {
                    if (current.Next == tail)
                        tail = current;
                    
                    current.Next = current.Next.Next;
                    size--;
                    return true;
                }
                current = current.Next;
            }
            
            return false;
        }
        
        // Remover en una posición específica
        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= size)
                return false;
            
            if (index == 0)
            {
                head = head.Next;
                if (head == null)
                    tail = null;
                size--;
                return true;
            }
            
            Node<T> current = head;
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }
            
            if (current.Next == tail)
                tail = current;
            
            current.Next = current.Next.Next;
            size--;
            return true;
        }
        
        // Verificar si contiene un elemento
        public bool Contains(T data)
        {
            Node<T> current = head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }
        
        // Obtener elemento en una posición
        public T GetAt(int index)
        {
            if (index < 0 || index >= size)
                throw new System.IndexOutOfRangeException("Índice fuera de rango");
            
            Node<T> current = head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            
            return current.Data;
        }
        
        // Establecer elemento en una posición
        public void SetAt(int index, T data)
        {
            if (index < 0 || index >= size)
                throw new System.IndexOutOfRangeException("Índice fuera de rango");
            
            Node<T> current = head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            
            current.Data = data;
        }
        
        // Obtener el tamaño de la lista
        public int Size()
        {
            return size;
        }
        
        // Verificar si está vacía
        public bool IsEmpty()
        {
            return size == 0;
        }
        
        // Limpiar la lista
        public void Clear()
        {
            head = null;
            tail = null;
            size = 0;
        }
        
        // Obtener el primer elemento sin removerlo
        public T GetFirst()
        {
            if (head == null)
                throw new System.InvalidOperationException("La lista está vacía");
            return head.Data;
        }
        
        // Obtener el último elemento sin removerlo
        public T GetLast()
        {
            if (tail == null)
                throw new System.InvalidOperationException("La lista está vacía");
            return tail.Data;
        }
        
        // Obtener iterador
        public IIterator<T> GetIterator()
        {
            return new LinkedListIterator<T>(head);
        }
        
        // Convertir a array (útil para debugging)
        public T[] ToArray()
        {
            T[] array = new T[size];
            Node<T> current = head;
            int index = 0;
            
            while (current != null)
            {
                array[index++] = current.Data;
                current = current.Next;
            }
            
            return array;
        }
        
        // Obtener índice de un elemento
        public int IndexOf(T data)
        {
            Node<T> current = head;
            int index = 0;
            
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return index;
                current = current.Next;
                index++;
            }
            
            return -1; // No encontrado
        }
    }
    
    // ============================================
    // QUEUE (COLA) IMPLEMENTACIÓN
    // ============================================
    public class Queue<T>
    {
        private LinkedList<T> list;
        
        public Queue()
        {
            list = new LinkedList<T>();
        }
        
        // Agregar al final de la cola
        public void Enqueue(T data)
        {
            list.Add(data);
        }
        
        // Remover del inicio de la cola
        public T Dequeue()
        {
            if (IsEmpty())
                throw new System.InvalidOperationException("La cola está vacía");
            
            T data = list.GetFirst();
            list.RemoveAt(0);
            return data;
        }
        
        // Ver el primer elemento sin removerlo
        public T Peek()
        {
            if (IsEmpty())
                throw new System.InvalidOperationException("La cola está vacía");
            
            return list.GetFirst();
        }
        
        // Verificar si está vacía
        public bool IsEmpty()
        {
            return list.IsEmpty();
        }
        
        // Obtener el tamaño
        public int Size()
        {
            return list.Size();
        }
        
        // Limpiar la cola
        public void Clear()
        {
            list.Clear();
        }
    }
    
    // ============================================
    // STACK (PILA) IMPLEMENTACIÓN
    // ============================================
    public class Stack<T>
    {
        private LinkedList<T> list;
        
        public Stack()
        {
            list = new LinkedList<T>();
        }
        
        // Agregar al inicio de la pila
        public void Push(T data)
        {
            list.AddFirst(data);
        }
        
        // Remover del inicio de la pila
        public T Pop()
        {
            if (IsEmpty())
                throw new System.InvalidOperationException("La pila está vacía");
            
            T data = list.GetFirst();
            list.RemoveAt(0);
            return data;
        }
        
        // Ver el primer elemento sin removerlo
        public T Peek()
        {
            if (IsEmpty())
                throw new System.InvalidOperationException("La pila está vacía");
            
            return list.GetFirst();
        }
        
        // Verificar si está vacía
        public bool IsEmpty()
        {
            return list.IsEmpty();
        }
        
        // Obtener el tamaño
        public int Size()
        {
            return list.Size();
        }
        
        // Limpiar la pila
        public void Clear()
        {
            list.Clear();
        }
    }
    
    // ============================================
    // HASHMAP IMPLEMENTACIÓN BÁSICA
    // ============================================
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
        }
        
        private LinkedList<Entry>[] buckets;
        private int size;
        private const int INITIAL_CAPACITY = 16;
        
        public HashMap()
        {
            buckets = new LinkedList<Entry>[INITIAL_CAPACITY];
            size = 0;
            
            for (int i = 0; i < INITIAL_CAPACITY; i++)
            {
                buckets[i] = new LinkedList<Entry>();
            }
        }
        
        // Función hash simple
        private int GetBucketIndex(K key)
        {
            int hashCode = key.GetHashCode();
            return System.Math.Abs(hashCode) % buckets.Length;
        }
        
        // Agregar o actualizar un valor
        public void Put(K key, V value)
        {
            int index = GetBucketIndex(key);
            LinkedList<Entry> bucket = buckets[index];
            
            // Buscar si ya existe la clave
            IIterator<Entry> iterator = bucket.GetIterator();
            while (iterator.HasNext())
            {
                Entry entry = iterator.Next();
                if (entry.Key.Equals(key))
                {
                    entry.Value = value; // Actualizar valor existente
                    return;
                }
            }
            
            // Si no existe, agregar nueva entrada
            bucket.Add(new Entry(key, value));
            size++;
        }
        
        // Obtener un valor
        public V Get(K key)
        {
            int index = GetBucketIndex(key);
            LinkedList<Entry> bucket = buckets[index];
            
            IIterator<Entry> iterator = bucket.GetIterator();
            while (iterator.HasNext())
            {
                Entry entry = iterator.Next();
                if (entry.Key.Equals(key))
                    return entry.Value;
            }
            
            return default(V); // Retornar valor por defecto si no se encuentra
        }
        
        // Verificar si contiene una clave
        public bool ContainsKey(K key)
        {
            int index = GetBucketIndex(key);
            LinkedList<Entry> bucket = buckets[index];
            
            IIterator<Entry> iterator = bucket.GetIterator();
            while (iterator.HasNext())
            {
                Entry entry = iterator.Next();
                if (entry.Key.Equals(key))
                    return true;
            }
            
            return false;
        }
        
        // Remover una entrada
        public bool Remove(K key)
        {
            int index = GetBucketIndex(key);
            LinkedList<Entry> bucket = buckets[index];
            
            IIterator<Entry> iterator = bucket.GetIterator();
            while (iterator.HasNext())
            {
                Entry entry = iterator.Next();
                if (entry.Key.Equals(key))
                {
                    bucket.Remove(entry);
                    size--;
                    return true;
                }
            }
            
            return false;
        }
        
        // Obtener el tamaño
        public int Size()
        {
            return size;
        }
        
        // Verificar si está vacío
        public bool IsEmpty()
        {
            return size == 0;
        }
        
        // Limpiar el mapa
        public void Clear()
        {
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i].Clear();
            }
            size = 0;
        }
        
        // Obtener todas las claves
        public LinkedList<K> GetKeys()
        {
            LinkedList<K> keys = new LinkedList<K>();
            
            for (int i = 0; i < buckets.Length; i++)
            {
                IIterator<Entry> iterator = buckets[i].GetIterator();
                while (iterator.HasNext())
                {
                    Entry entry = iterator.Next();
                    keys.Add(entry.Key);
                }
            }
            
            return keys;
        }
        
        // Obtener todos los valores
        public LinkedList<V> GetValues()
        {
            LinkedList<V> values = new LinkedList<V>();
            
            for (int i = 0; i < buckets.Length; i++)
            {
                IIterator<Entry> iterator = buckets[i].GetIterator();
                while (iterator.HasNext())
                {
                    Entry entry = iterator.Next();
                    values.Add(entry.Value);
                }
            }
            
            return values;
        }
    }
}