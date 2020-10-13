using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WRMyHashTableClass
{
	public class MyHashtable<TKey, TValue> : IEnumerable<Item<TKey, TValue>> where TValue : class
	{
		public int ListSize { get; private set; } = 4;
		public int ListMaxSize { get; private set; } = 1;
		public int CountOfItems { get; private set; } = 0;
		public int StorageTime { get; private set; }
		LinkedList<Item<TKey, TValue>>[] listOfItems;

		public int HashFunc(TKey key, int Size) => Math.Abs((key.GetHashCode() % 5) % Size);

		public MyHashtable(int storageTime) : this()
		{
			StorageTime = storageTime;
		}

		public MyHashtable()
		{
			listOfItems = new LinkedList<Item<TKey, TValue>>[ListSize];
			StorageTime = 10000;
		}

		public void Resize()
		{
			int checkListSize = ListSize * 2;
			int checkListMaxSize = checkListSize / 4;
			var checkList = new LinkedList<Item<TKey, TValue>>[checkListSize];

			foreach (var item in this)
			{
				int index = HashFunc(item.Key, ListSize);
				if (checkList[index] == null)
					checkList[index] = new LinkedList<Item<TKey, TValue>>();
				checkList[index].AddLast(item);
			}

			listOfItems = checkList;
			ListSize = checkListSize;
			ListMaxSize = checkListMaxSize;
		}

		public void MyAdd(TKey key, TValue value)
		{
			if (listOfItems == null)
				listOfItems = new LinkedList<Item<TKey, TValue>>[ListSize];
			if (!KeyExistence(key))
			{
				int index = HashFunc(key, ListSize);
				if (listOfItems[index] == null)
				{
					listOfItems[index] = new LinkedList<Item<TKey, TValue>>();
				}
				listOfItems[index].AddLast(new Item<TKey, TValue>(StorageTime, key, value));

				if (listOfItems[index].Count > ListMaxSize)
				{
					Resize();
				}
			}
		}

		public void MyDelete(TKey key)
		{
			if (KeyExistence(key))
			{
				int index = HashFunc(key, ListSize);
				var removableItem = listOfItems[index].First;
				while (removableItem != null)
				{
					if (removableItem.Value.Key.Equals(key))
					{
						listOfItems[index].Remove(removableItem);
						CountOfItems -= CountOfItems;
						break;
					}
					removableItem = removableItem.Next;
				}
			}
		}

		public bool MySearch(TKey key, out TValue value)
		{
			value = default;
			if (KeyExistence(key))
			{
				int index = HashFunc(key, ListSize);
				if (listOfItems[index] != null)
				{
					var requireItem = listOfItems[index].First;
					while (requireItem != null)
					{
						if (requireItem.Value.Key.Equals(key))
						{
							if (requireItem.Value.Value.TryGetTarget(out TValue target))
							{
								value = target;
								return true;
							}
						}
						requireItem = requireItem.Next;
					}
				}
			}
			return false;
		}

		public void Clear()
		{
			listOfItems = null;
			ListSize = 4;
			ListMaxSize = 1;
		}

		public bool KeyExistence(TKey key)
		{
			int index = HashFunc(key, ListSize);

			if (listOfItems[index] != null)
			{
				var checkItem = listOfItems[index].First;
				while (checkItem != null)
				{
					if (checkItem.Value.Key.Equals(key))
					{
						return true;
					}
					checkItem = checkItem.Next;
				}
			}
			return false;
		}

		public bool ValueExistence(TValue value)
		{
			return this.Count(item => item.Value.TryGetTarget(out TValue _value) && value.Equals(value)) != 0;
		}

		public IEnumerator<Item<TKey, TValue>> GetEnumerator()
		{
			for (int i = 0; i < ListSize; i++)
			{
				if (listOfItems[i] != null)
				{
					var checkItem = listOfItems[i].First;
					while (checkItem != null)
					{
						if (checkItem.Value.Value.TryGetTarget(out _))
						{
							yield return checkItem.Value;
						}
						checkItem = checkItem.Next;
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int NumOfPairs()
		{
			return this.Where(node => node.Value.TryGetTarget(out _) == true).Count();
		}

		public void Print()
		{
			foreach (var item in this)
			{
				Console.WriteLine(item);
			}
		}
	}

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