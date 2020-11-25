using System;
using System.Threading;
using System.Threading.Tasks;
using WeakReference.WeakBinaryTree;

namespace WeakReference
{
    public class Program
    {
        static WeakBinaryTree<string> weakBinaryTree = new WeakBinaryTree<string>(5000);
        static void TestWeakBinaryTree()
        {
            var obj1 = new WeakReference<String>("1");
            var obj2 = new WeakReference<String>("4");
            var obj3 = new WeakReference<String>("9");

            weakBinaryTree.Add(obj1);
            weakBinaryTree.Add(obj2);
            weakBinaryTree.Add(obj3);
        }
        static void Main(string[] args)
        {
            TestWeakBinaryTree();
            Console.WriteLine("Wait 0 second");
            weakBinaryTree.Traverse(weakBinaryTree.Root);

            Thread.Sleep(2000);
            GC.Collect();

            TestWeakBinaryTree();
            Console.WriteLine("Wait 2 seconds");
            weakBinaryTree.Traverse(weakBinaryTree.Root);

            Thread.Sleep(4000);
            GC.Collect();

            TestWeakBinaryTree();
            Console.WriteLine("Wait 4 seconds");
            weakBinaryTree.Traverse(weakBinaryTree.Root);

            Thread.Sleep(6000);
            GC.Collect();

            Console.WriteLine("Wait 6 seconds");
            weakBinaryTree.Traverse(weakBinaryTree.Root);
        }
    }
}