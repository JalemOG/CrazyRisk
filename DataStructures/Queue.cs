using System;

namespace CrazyRisk.DataStructures
{
    public class Queue<T>
    {
        private Node<T> front;
        private Node<T> rear;
        private int count;
        
        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }
        
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
    }
}