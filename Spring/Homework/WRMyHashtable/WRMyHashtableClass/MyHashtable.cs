using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WRMyHashtableItem;

namespace WRMyHashTableClass
{
	public class MyHashtable<TKey, TValue> : IEnumerable<Item<TKey, TValue>> where TValue : class
	{
		public int listSize { get; private set; } = 4;
		public int listMaxSize { get; private set; } = 1;
		public int countOfItems { get; private set; } = 0;
		public int StorageTime { get; private set; }
		LinkedList<Item<TKey, TValue>>[] listOfItems;

		public int HashFunc(TKey key, int Size) => Math.Abs((key.GetHashCode() % 5) % Size);

		public MyHashtable(int storageTime) : this()
		{
			StorageTime = storageTime;
		}

		public MyHashtable()
		{
			listOfItems = new LinkedList<Item<TKey, TValue>>[listSize];
			StorageTime = 10000;
		}

		public void Resize()
		{
			int checkListSize = listSize * 2;
			int checkListMaxSize = checkListSize / 4;
			var checkList = new LinkedList<Item<TKey, TValue>>[checkListSize];

			foreach (var item in this)
			{
				int index = HashFunc(item.Key, listSize);
				if (checkList[index] == null)
					checkList[index] = new LinkedList<Item<TKey, TValue>>();
				checkList[index].AddLast(item);
			}

			listOfItems = checkList;
			listSize = checkListSize;
			listMaxSize = checkListMaxSize;
		}

		public void MyAdd(TKey key, TValue value)
		{
			if (listOfItems == null)
				listOfItems = new LinkedList<Item<TKey, TValue>>[listSize];
			if (!KeyExistence(key))
			{
				int index = HashFunc(key, listSize);
				if (listOfItems[index] == null)
				{
					listOfItems[index] = new LinkedList<Item<TKey, TValue>>();
				}
				listOfItems[index].AddLast(new Item<TKey, TValue>(StorageTime, key, value));

				if (listOfItems[index].Count > listMaxSize)
				{
					Resize();
				}
			}
		}

		public void MyDelete(TKey key)
		{
			if (KeyExistence(key))
			{
				int index = HashFunc(key, listSize);
				var removableItem = listOfItems[index].First;
				while (removableItem != null)
				{
					if (removableItem.Value.Key.Equals(key))
					{
						listOfItems[index].Remove(removableItem);
						countOfItems -= countOfItems;
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
				int index = HashFunc(key, listSize);
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
			listSize = 4;
			listMaxSize = 1;
		}

		public bool KeyExistence(TKey key)
		{
			int index = HashFunc(key, listSize);

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
			for (int i = 0; i < listSize; i++)
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
}