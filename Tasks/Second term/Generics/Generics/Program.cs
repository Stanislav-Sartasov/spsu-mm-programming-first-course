using System;
using DynamicArray;

namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            //arrange
            DynamicArray<int> dynamicArray = new DynamicArray<int>();

            //act
            dynamicArray.Add(1);
            dynamicArray.Add(2);
            dynamicArray.Add(3);
            dynamicArray.Delete(1);
            int actual = dynamicArray.GetAtIndex(1);
            int expected = 2;
            Console.WriteLine(actual);
            DynamicArray<int>.PrintArray(dynamicArray);
        }
    }
}
