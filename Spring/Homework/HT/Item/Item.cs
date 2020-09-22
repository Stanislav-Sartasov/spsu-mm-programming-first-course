namespace HTItem
{
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
