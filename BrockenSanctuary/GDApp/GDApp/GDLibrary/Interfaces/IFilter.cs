namespace GDLibrary
{
    //used by search and remove methods
    public interface IFilter<T>
    {
        bool Matches(T obj);
    }
}
