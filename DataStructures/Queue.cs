using System;

namespace CrazyRisk.DataStructures
{
    public class Queue<T>
    {
        private Node<T> front;
        private Node<T> rear;
        private int count;
        
        public int Count => count;
        public bool IsEmpty => count == 0;
        
        public void Enqueue(T value)
        {
            Node<T> newNode = new Node<T>(value);
            
            if (rear == null)
            {
                front = rear = newNode;
            }
            else
            {
                rear.Next = newNode;
                rear = newNode;
            }
            count++;
        }
        
        public T Dequeue()
        {
            if (front == null)
                throw new InvalidOperationException("Queue is empty");
                
            T value = front.Value;
            front = front.Next;
            
            if (front == null)
                rear = null;
                
            count--;
            return value;
        }
        
        public T Peek()
        {
            if (front == null)
                throw new InvalidOperationException("Queue is empty");
                
            return front.Value;
        }

        // Iterador para compatibilidad con IIterator<T>
        public IIterator<T> GetIterator()
        {
            return new QueueIterator(this);
        }

        private class QueueIterator : IIterator<T>
        {
            private Node<T> current;

            public QueueIterator(Queue<T> queue)
            {
                current = queue.front;
            }

            public bool HasNext() => current != null;

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
