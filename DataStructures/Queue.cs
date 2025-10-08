using System;

namespace CrazyRisk.DataStructures
{
    public class Queue<T>
    {
        private Node<T>? front;
        private Node<T>? rear;
        private int count;

        public int Count => count;
        public bool IsEmpty => count == 0;

        public void Enqueue(T value)
        {
            var n = new Node<T>(value);
            if (rear is null)
            {
                front = rear = n;
            }
            else
            {
                rear.Next = n;
                rear = n;
            }
            count++;
        }

        public T Dequeue()
        {
            if (front is null)
                throw new InvalidOperationException("Queue vacía");

            var val = front.Value;
            front = front.Next;
            if (front is null) rear = null; // quedó vacía
            count--;
            return val;
        }

        public T Peek()
        {
            if (front is null)
                throw new InvalidOperationException("Queue vacía");

            return front.Value;
        }

        public void Clear()
        {
            front = null;
            rear = null;
            count = 0;
        }
    }
}