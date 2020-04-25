using NUnit.Framework;
using System;
using Task4;

namespace Task4Tests
{
    public class Tests
    {
        [Test]
        public void TestIntFrom0To2000000()
        {
            HashTable<int, int> hashTable = new HashTable<int, int>();
            for (int i = 0; i < 2000000; i++)
                hashTable.Add(i, i);
            for (int i = 0; i < 2000000; i++)
            {
                Assert.AreEqual(i, hashTable.Find(i));
                Assert.AreEqual(true, hashTable.Remove(i));
                Assert.AreEqual(false, hashTable.Remove(i));
            }
        }
        [Test]
        public void TestStringFromAToZZZZ()
        {
            string s = "A";
            int ind = 0;
            HashTable<string, int> hashTable = new HashTable<string, int>();
            while (s != "ZZZZ")
            {
                hashTable.Add(s, ind++);
                s = (char)(s[0] + 1) + s.Substring(1);
                for (int i = 0; i < s.Length && s[i] == 'Z' + 1; i++)
                {
                    s = s.Substring(0, i) + 'A' + s.Substring(i + 1);
                    if (i + 1 == s.Length)
                        s += 'A';
                    else
                        s = s.Substring(0, i + 1) + (char)(s[i + 1] + 1) + s.Substring(i + 2);
                }
            }
            s = "A";
            ind = 0;
            while (s != "ZZZZ")
            {
                Assert.AreEqual(ind++, hashTable.Find(s));
                Assert.AreEqual(true, hashTable.Remove(s));
                Assert.AreEqual(false, hashTable.Remove(s));
                s = (char)(s[0] + 1) + s.Substring(1);
                for (int i = 0; i < s.Length && s[i] == 'Z' + 1; i++)
                {
                    s = s.Substring(0, i) + 'A' + s.Substring(i + 1);
                    if (i + 1 == s.Length)
                        s += 'A';
                    else
                        s = s.Substring(0, i + 1) + (char)(s[i + 1] + 1) + s.Substring(i + 2);
                }
            }
        }
    }
}