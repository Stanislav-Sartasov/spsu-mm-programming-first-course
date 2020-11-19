using System;
using System.Threading.Tasks;

namespace Item
{
	public class Item<TKey, TValue> where TValue : class
	{
		public TKey Key { get; private set; }
		public WeakReference<TValue> Value { get; private set; }
		private int storageTime;
		public Item(int storageTime, TKey key, TValue value)
		{
			this.storageTime = storageTime;
			SetPair(key, value);
		}

		public Item(TKey key, TValue value)
		{
			Key = key;
			Value = new WeakReference<TValue>(value);
		}
		async public void SetPair(TKey key, TValue value)
		{
			Key = key;
			Value = new WeakReference<TValue>(value);
			await Task.Delay(storageTime);
		}

		public override string ToString()
		{
			if (Value.TryGetTarget(out TValue target))
				return $"{Key}, {target}";
			else
				return $"{Key}, collected";
		}
	}
}
