using System;
using System.Collections.Generic;
using System.Text;

namespace WeakReference
{
    public class Node<T> where T: class
    {
        public Node(int _key, T _value)
        {
            key = _key;
            value = new WeakReference<T>(_value);
        }
        public bool IsAlive()
        {
            return value.TryGetTarget(out T target);
        }
        public T GetValue()
        {
            value.TryGetTarget(out T target);
            return target;
        }
        public void SetValue(T newValue)
        {
            value.SetTarget(newValue);
        }



        public WeakReference<T> value;
        public int key;
        public Node<T> left;
        public Node<T> right;
        public void DisplayNode()
        {
            Console.WriteLine(key + " ");
        }
    }
}