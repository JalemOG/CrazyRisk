namespace CrazyRisk.DataStructures
{
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }

    public class LinkedListIterator<T> : IIterator<T>
    {
        private Node<T>? current;

        public LinkedListIterator(Node<T>? head)
        {
            current = head;
        }

        public bool HasNext() => current != null;


        public T Next()
        {
            if (current == null)
                throw new System.InvalidOperationException("No hay más elementos");

            T value = current.Value;
            current = current.Next;
            return value;
        }
    }
}
