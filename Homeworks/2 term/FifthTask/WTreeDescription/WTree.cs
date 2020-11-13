using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FifthTask.WTreeDescription
{
	public enum Side
	{
		Left,
		Right
	}

	public class WTree<T> where T : class
	{
		private int? Key { get; set; }
		private WeakReference<T> Data { get; set; }
		private int RetentionTime { get; set; }
		public WTree(int? time = null, int? key = null, T data = null)
		{
			if (time == null)
			{
				Key = key;
				Data = new WeakReference<T>(data);
			}
			else
			{
				RetentionTime = (int)time;
			}
		}

		private WTree<T> Left { get; set; }
		private WTree<T> Right { get; set; }
		private WTree<T> Parent { get; set; }
		private WTree<T> Root { get; set; }
		private static Side? ParentSide(WTree<T> node)
		{
			if (node.Parent != null)
			{
				if (node.Parent.Left == node)
				{
					return Side.Left;
				}
				else
				{
					return Side.Right;
				}

			}
			return null;
		}

		public T GetData()
		{
			if (Data.TryGetTarget(out T data))
			{
				return data;
			}
			return null;
		}

		public async void AddElement(int key, T data)
		{
			AddElementMethod(key, data);
			await Task.Delay(RetentionTime);
		}
		private void AddElementMethod(int? key = null, T data = null, WTree<T> node = null, WTree<T> parentNode = null)
		{
			if (node == null)
			{
				node = new WTree<T>(key: key, data: data);
			}

			if (Root == null)
			{
				node.Parent = null;
				Root = node;
				return;
			}

			if (parentNode == null)
			{
				parentNode = Root;
			}
			node.Parent = parentNode;

			if (node.Key == parentNode.Key)
			{
				parentNode.Data.SetTarget(node.GetData());
			}
			else
			{
				if (node.Key < parentNode.Key)
				{
					if (parentNode.Left == null)
					{
						parentNode.Left = node;
					}
					else
					{
						AddElementMethod(node: node, parentNode: parentNode.Left);
					}
				}
				else
				{
					if (parentNode.Right == null)
					{
						parentNode.Right = node;
					}
					else
					{
						AddElementMethod(node: node, parentNode: parentNode.Right);
					}
				}
			}
		}

		public void RemoveElement(int key)
		{
			var node = SearchElement(key);
			if (node == null)
			{
				return;
			}
			var nodeSide = ParentSide(node);

			if (node.Left == null && node.Right == null)
			{
				if (nodeSide == Side.Left)
				{
					node.Parent.Left = null;
				}
				else
				{
					node.Parent.Right = null;
				}

				return;
			}

			if (node.Left != null && node.Right == null)
			{
				if (nodeSide == Side.Left)
				{
					node.Parent.Left = node.Left;
				}
				else
				{
					node.Parent.Right = node.Left;
				}

				node.Left.Parent = node.Parent;
				return;
			}

			if (node.Left == null && node.Right != null)
			{
				if (nodeSide == Side.Left)
				{
					node.Parent.Left = node.Right;
				}
				else
				{
					node.Parent.Right = node.Right;
				}

				node.Right.Parent = node.Parent;
				return;
			}

			if (node.Right != null && node.Left != null)
			{
				if (nodeSide == null) // Root
				{
					var temp = node.Left;
					node.Key = node.Right.Key;
					node.Data = node.Right.Data;

					var tempLeft = node.Right.Left;
					var tempRight = node.Left.Right;
					node.Left = tempLeft;
					node.Right = tempRight;

					AddElementMethod(node: temp, parentNode: node);
				}
				else
				{
					if (nodeSide == Side.Left)
					{
						node.Parent.Left = node.Right;
						node.Right.Parent = node.Parent;

						AddElementMethod(node: node.Left, parentNode: node.Right);
					}
					else
					{
						node.Parent.Right = node.Right;
						node.Right.Parent = node.Parent;

						AddElementMethod(node: node.Left, parentNode: node.Right);
					}
				}
			}
		}

		public WTree<T> SearchElement(int key, WTree<T> searchNode = null)
		{
			if (searchNode == null)
			{
				searchNode = Root;
			}

			if (searchNode.Key == key && searchNode.Data.TryGetTarget(out T data))
			{
				return searchNode;
			}
			else
			{
				if (searchNode.Key > key)
				{
					if (searchNode.Left != null)
					{
						return SearchElement(key, searchNode.Left);
					}
					else
					{
						return null;
					}
				}
				else
				{
					if (searchNode.Right != null)
					{
						return SearchElement(key, searchNode.Right);
					}
					else
					{
						return null;
					}
				}
			}
		}
	}
}