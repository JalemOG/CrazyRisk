using System;

namespace CrazyRisk.DataStructures
{
    public class Stack<T>
    {
        private Node<T>? top;
        private int count;

        public int Count => count;
        public bool IsEmpty => count == 0;

        public void Push(T value)
        {
            var n = new Node<T>(value) { Next = top };
            top = n;
            count++;
        }

        public T Pop()
        {
            if (top is null)
                throw new InvalidOperationException("Stack vacío");

            var val = top.Value;
            top = top.Next;   // puede quedar null (OK: top es nullable)
            count--;
            return val;
        }

        public T Peek()
        {
            if (top is null)
                throw new InvalidOperationException("Stack vacío");

            return top.Value;
        }

        public void Clear()
        {
            top = null;
            count = 0;
        }
    }
}