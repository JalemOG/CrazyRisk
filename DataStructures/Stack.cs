using System;

namespace CrazyRisk.DataStructures
{
    public class Stack<T>
    {
        private Node<T> top;
        private int count;
        
        public int Count => count;
        public bool IsEmpty => count == 0;
        
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

        // Iterador para compatibilidad con IIterator<T>
        public IIterator<T> GetIterator()
        {
            return new StackIterator(this);
        }

        private class StackIterator : IIterator<T>
        {
            private Node<T> current;

            public StackIterator(Stack<T> stack)
            {
                current = stack.top;
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
