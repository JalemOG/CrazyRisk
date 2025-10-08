using System;

namespace CrazyRisk.DataStructures
{
    public class LinkedList<T>
    {
        private Node<T>? head;

        // Para que el iterador inicie desde el primer nodo
        internal Node<T>? GetHead() => head;
        private int count;

        public int Count => count;
        public bool IsEmpty => count == 0;
        
        // Agrega al final
        public void Add(T value)
        {
            var newNode = new Node<T>(value);

            if (head == null)
            {
                head = newNode;
            }
            else
            {
                var current = head;
                while (current.Next != null)
                    current = current.Next;

                current.Next = newNode;
            }

            count++;
        }

        // Elimina la primera ocurrencia de 'value'
        public bool Remove(T value)
        {
            if (head == null) return false;

            if (object.Equals(head.Value, value))
            {
                head = head.Next;
                count--;
                return true;
            }

            var current = head;
            while (current.Next != null && !object.Equals(current.Next.Value, value))
                current = current.Next;

            if (current.Next == null) return false;

            current.Next = current.Next.Next;
            count--;
            return true;
        }

        public bool Contains(T value)
        {
            var current = head;
            while (current != null)
            {
                if (object.Equals(current.Value, value))
                    return true;
                current = current.Next;
            }
            return false;
        }

        // Iterador (definido en Iiterator.cs)
        public IIterator<T> GetIterator()
        {
            return new LinkedListIterator<T>(head);
        }

        // === utilidades usadas por HashMap/NeutralArmy/Map ===

        // Deja la lista vacía
        public void Clear()
        {
            head = null;
            count = 0;
        }

        // Primer elemento que cumpla; default si ninguno
        public T? Find(Predicate<T> match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            var current = head;
            while (current != null)
            {
                if (match(current.Value))
                    return current.Value;
                current = current.Next;
            }
            return default;
        }

        // Compat con llamados existentes
        public int Size() => count;

        // Necesario para NeutralArmy (antes usabas LINQ ToArray)
        public T[] ToArray()
        {
            var arr = new T[count];
            int i = 0;
            var current = head;
            while (current != null)
            {
                arr[i++] = current.Value;
                current = current.Next;
            }
            return arr;
        }
    }
}