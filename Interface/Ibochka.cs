namespace Bochka_DS.Interface
{
    public interface Ibochka<T> : IEnumerable<T>, ICollection<T>, IList<T>
    {
        string Stringify();
        void Print();
    }
}
