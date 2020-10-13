﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HTClass
{
	public class MyHashtable<TKey, TValue> : IEnumerable<Item<TKey, TValue>>
	{
		public int ListSize { get; private set; } = 4;
		public int ListMaxSize { get; private set; } = 1;
		public int CountOfItems { get; private set; } = 0;
		LinkedList<Item<TKey, TValue>>[] listOfItems;

		public int HashFunc(TKey key, int Size) => Math.Abs(key.GetHashCode() % Size);

		public MyHashtable()
		{
			listOfItems = new LinkedList<Item<TKey, TValue>>[ListSize];
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
				listOfItems[index].AddLast(new Item<TKey, TValue>(key, value));
				CountOfItems += CountOfItems;

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
				var requiredKey = listOfItems[index].First;
				while (listOfItems[index] != null)
				{
					if (requiredKey.Value.Key.Equals(key))
					{
						value = requiredKey.Value.Value;
						return true;
					}
					requiredKey = requiredKey.Next;
				}
			}
			return false;
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
			return this.Count(item => item.Value.Equals(value)) != 0;
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
						yield return checkItem.Value;
						checkItem = checkItem.Next;
					}
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Print()
		{
			foreach (var item in this)
				Console.WriteLine(item);
		}
	}

	public class Item<TKey, TValue>
	{
		public TKey Key { get; private set; }
		public TValue Value { get; private set; }
		public Item(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		public override string ToString()
		{
			return $"{Key}, {Value}";
		}
	}
}