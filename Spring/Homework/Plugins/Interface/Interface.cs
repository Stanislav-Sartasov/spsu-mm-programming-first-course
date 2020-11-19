namespace Interface
{
	public interface Interface<T>
	{
		T Get();
		void Set(T value);
		string GetInfo();
	}
}
