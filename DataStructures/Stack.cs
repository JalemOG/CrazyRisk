using System;

namespace CrazyRisk.DataStructures
{
    public class Stack<T>
    {
        private Node<T> top;
        private int count;
        
        public int Count { get { return count; } }
        public bool IsEmpty { get { return count == 0; } }
        
        public void Push(T value)
        {
            Node<T> newNode = new Node<T>(value);
            newNode.Next = top;
            top = newNode;
            count++;
        }
        
        public T Pop()
        {
            if (top == null)
                throw new InvalidOperationException("Stack is empty");
                
            T value = top.Value;
            top = top.Next;
            count--;
            return value;
        }
        
        public T Peek()
        {
            if (top == null)
                throw new InvalidOperationException("Stack is empty");
                
            return top.Value;
        }
    }
}