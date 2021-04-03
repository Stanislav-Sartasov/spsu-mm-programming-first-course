
namespace Task3
{
	public class Product
	{
		public Product(string dealerId, string productId)
		{
			this.dealerId = dealerId;
			this.productId = productId;
		}

		public static readonly Product Default = new Product("Empty", "Empty");

		string dealerId;
		string productId;
		public string DealerId
		{
			get
			{
				return (string)dealerId.Clone();
			}
		}
		public string ProductId
		{
			get
			{
				return (string)productId.Clone();
			}
		}
	}
}
