namespace CrazyRisk.DataStructures
{
    public static class LinkedListExtensions
    {
        public static int Size<T>(this LinkedList<T> list) => list.Count;
    }
}