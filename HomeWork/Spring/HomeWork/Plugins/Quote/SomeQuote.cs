using System;
using MyInterface;

namespace Quote
{
    public class SomeQuote : IDisplayText
    {
        public string Text()
        {
            return "Don't know. Dont' care. Not my problem";
        }
    }
}
