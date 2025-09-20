using System;

namespace CrazyRisk.DataStructures
{
    public class LinkedList<T>
    {
        private Node<T> head;
        private int count;
        
        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }
        
        internal Node<T> GetHead()
        {
            return head;
        }
        
        public void Add(T value)
        {
            Node<T> newNode = new Node<T>(value);
            
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node<T> current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
            count++;
        }
        
        public bool Remove(T value)
        {
            if (head == null) return false;
            
            if (head.Value.Equals(value))
            {
                head = head.Next;
                count--;
                return true;
            }
            
            Node<T> current = head;
            while (current.Next != null)
            {
                if (current.Next.Value.Equals(value))
                {
                    current.Next = current.Next.Next;
                    count--;
                    return true;
                }
                current = current.Next;
            }
            
            return false;
        }
        
        public Node<T> Find(T value)
        {
            Node<T> current = head;
            while (current != null)
            {
                if (current.Value.Equals(value))
                    return current;
                current = current.Next;
            }
            return null;
        }
        
        public bool Contains(T value)
        {
            return Find(value) != null;
        }
        
        public T[] ToArray()
        {
            T[] array = new T[count];
            Node<T> current = head;
            int index = 0;
            
            while (current != null)
            {
                array[index++] = current.Value;
                current = current.Next;
            }
            
            return array;
        }
        
        public IIterator<T> GetIterator()
        {
            return new LinkedListIterator<T>(this);
        }
        
        private class LinkedListIterator<T> : IIterator<T>
        {
            private Node<T> current;
            
            public LinkedListIterator(LinkedList<T> list)
            {
                current = list.GetHead();
            }
            
            public bool HasNext()
            {
                return current != null;
            }
            
            public T Next()
            {
                if (current == null)
                    throw new InvalidOperationException("No more elements");
                    
                T value = current.Value;
                current = current.Next;
                return value;
            }
        }
    }
}