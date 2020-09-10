using System;
using System.Collections.Generic;
using System.Text;

namespace TreeDescription
{
        public enum Side
        {
                Left,
                Right
        }

        public class BinaryTree<T>
        {
                public int? Key { get; private set; }
                public T Data { get; private set; }
                public BinaryTree<T> Left { get; private set; } // Left tree
                public BinaryTree<T> Right { get; private set; } // Right tree
                public BinaryTree<T> Parent { get; private set; } // Parent tree
                private static Side? ParentSide(BinaryTree<T> node)
                {
                        if (node.Parent != null)
                        {
                                if (node.Parent.Left == node)
                                {
                                        return Side.Left;
                                }
                                if (node.Parent.Right == node)
                                {
                                        return Side.Right;
                                }
                        }
                        return null;
                }

                public void AddElement(int key, T data, BinaryTree<T> node = null, BinaryTree<T> parent = null)
                {
                        if (node == null)
                        {
                                node = this;
                        }

                        if (parent == null)
                        {
                                parent = this.Parent;
                        }

                        if (node.Key == null || node.Key == key)
                        {
                                if (node.Key == null)
                                {
                                        node.Left = null;
                                        node.Right = null;
                                }
                                else
                                {
                                        node.Left = this.Left;
                                        node.Right = this.Right;
                                }

                                node.Key = key;
                                node.Data = data;
                                node.Parent = parent;
                                return;
                        }

                        if (node.Key > key)
                        {
                                if (node.Left == null)
                                {
                                        node.Left = new BinaryTree<T>();
                                }
                                AddElement(key, data, node.Left, node);
                        }
                        else
                        {
                                if (node.Right == null)
                                {
                                        node.Right = new BinaryTree<T>();
                                }
                                AddElement(key, data, node.Right, node);
                        }
                }

                public void RemoveElement(int key)
                {

                        var node = SearchElement(key);

                        if (node == null)
                        {
                                return;
                        }

                        if (node == this)
                        {
                                BinaryTree<T> currNode;
                                if (node.Right != null)
                                {
                                        currNode = node.Right;
                                }
                                else
                                {
                                        currNode = node.Left;
                                }

                                while (currNode.Left != null)
                                {
                                        currNode = currNode.Left;
                                }

                                int tempKey = (int)currNode.Key;
                                T tempData = currNode.Data;

                                this.RemoveElement(tempKey);

                                node.Key = tempKey;
                                node.Data = tempData;
                                return;
                        }

                        if (node.Left == null && node.Right == null)
                        {
                                var nodeSide = ParentSide(node);

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
                                var nodeSide = ParentSide(node);

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
                                var nodeSide = ParentSide(node);

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
                                var currNode = node.Right;

                                while (currNode.Left != null)
                                {
                                        currNode = currNode.Left;
                                }

                                if (currNode.Parent == node)
                                {
                                        currNode.Left = node.Left;
                                        node.Left.Parent = currNode;
                                        currNode.Parent = node.Parent;

                                        var nodeSide = ParentSide(node);

                                        if (nodeSide == Side.Left)
                                        {
                                                node.Parent.Left = currNode;
                                        }
                                        else
                                        {
                                                node.Parent.Right = currNode;
                                        }

                                        return;
                                }
                                else
                                {
                                        if (currNode.Right != null)
                                        {
                                                currNode.Right.Parent = currNode.Parent;
                                        }

                                        currNode.Parent.Left = currNode.Right;
                                        currNode.Right = node.Right;
                                        currNode.Left = node.Left;
                                        node.Left.Parent = currNode;
                                        node.Right.Parent = currNode;
                                        currNode.Parent = node.Parent;

                                        if (node == node.Parent.Left)
                                        {
                                                node.Parent.Left = currNode;
                                        }
                                        else
                                        {
                                                node.Parent.Right = currNode;
                                        }

                                        return;
                                }
                        }
                }

                public BinaryTree<T> SearchElement(int key, /*int isPrint = 0,*/ BinaryTree<T> searchNode = null)
                {
                        if (searchNode == null)
                        {
                                searchNode = this;
                        }

                        if (searchNode.Key == key)
                        {
                                /*
                                if (isPrint == 1)
                                    Console.WriteLine($"{searchNode.Key}: {searchNode.Data}");
                                */

                                return searchNode;
                        }

                        if (searchNode.Key > key)
                        {
                                if (searchNode.Left != null)
                                {
                                        return SearchElement(key, /*isPrint,*/ searchNode.Left);
                                }
                                else
                                {
                                        /*
                                        if (isPrint == 1)
                                            Console.WriteLine("Not found");
                                        */

                                        return null;
                                }
                        }
                        else
                        {
                                if (searchNode.Right != null)
                                {
                                        return SearchElement(key, /*isPrint,*/ searchNode.Right);
                                }
                                else
                                {
                                        /*
                                        if (isPrint == 1)
                                            Console.WriteLine("Not found");
                                        */

                                        return null;
                                }
                        }
                }
        }
}
