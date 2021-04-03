using System.Collections.Generic;

namespace Task3
{
	public class Depot<T>
	{
		Queue<T> storage = new Queue<T>();
		Locker locker = new Locker();

		public List<T> Storage
		{
			get
			{
				locker.Lock();
				List<T> local = new List<T>();
				foreach (var p in storage)
					local.Add(p);
				locker.Unlock();
				return local;
			}
		}
		public void Put(T product)
		{
			locker.Lock();
			storage.Enqueue(product);
			locker.Unlock();
		}
		public T Get()
		{
			locker.Lock();
			T product = storage.Count == 0 ? default : storage.Dequeue();
			locker.Unlock();
			return product;
		}
	}
}
