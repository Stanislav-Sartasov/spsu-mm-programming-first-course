using System;
using MyInterface;

namespace HelloWorld
{
    public class Hello : IDisplayText
    {
        public string Text()
        {
            return "Hello world";
        }
    }
}
