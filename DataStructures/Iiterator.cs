namespace CrazyRisk.DataStructures
{
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }
}