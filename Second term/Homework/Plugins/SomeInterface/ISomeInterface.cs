namespace SomeInterface
{
    public interface ISomeInterface<T>
    {
        T Get();
        void Set(T value);

        string GetInfo();
    }
}
