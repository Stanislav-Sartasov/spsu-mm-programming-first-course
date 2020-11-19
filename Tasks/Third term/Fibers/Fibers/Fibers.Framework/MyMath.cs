using System;
using System.Collections.Generic;
using System.Text;

namespace Fibers.Framework
{
    public static class MyMath
    {
        public static List<int> ListOfLcm(List<int> a)
        {
            List<int> ans = new List<int>();

            if (a.Count == 1)
                return new List<int>(a);
            a.Sort();
            int s = a[0];
            ans.Add(s);
            for(int i = 1; i < a.Count; i++)
            {
                s = (s * a[i]) / Gcd(s, a[i]);
                ans.Add(s);
            }

            return ans;
        }

        public static int Gcd(List<int> a)
        {
            int ans = 1;

            if (a.Count == 1)
                return a[0];
            int x = a[0];
            for (int i = 1; i < a.Count; i++)
            {
                ans = Gcd(x, a[i]);
            }

            return ans;
        }

        public static int Gcd(int a, int b)
        {
            int ans = 1;

            int c = 0;
            while (b != 0)
            {
                c = b;
                b = a % b;
                a = c;
            }
            ans = c;

            return ans;
        }
    }
}
