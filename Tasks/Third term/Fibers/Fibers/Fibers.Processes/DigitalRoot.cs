using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace Fibers.Processes
{
    class DigitalRoot : IProcess
    {
        private const int n = 1_000_000;
        private byte[] arrayOfRes = new byte[n];

        public async void Process()
        {
            int ans = 0;
            for (int i = 2; i < n; i++)
            {
                ans += FindMaxRoot(i);
                Console.WriteLine($"Root {ans}");
            }
            await Task.Delay(100);
            Console.WriteLine($"Sum of digital roots = {ans}");
        }

        private int FindMaxRoot(int y)
        {
            if (arrayOfRes[y] != 0)
                return arrayOfRes[y];
            int res = 1 + (y - 1) % 9;
            int i = 0;
            int j = y;
            int[] v = new int[20];
            int div = 0;
            while (j != 1 && div != 1)
            {
                div = FindNextDivider(ref j);
                if (div != 1)
                {
                    v[i] = div;
                    i++;
                }
            }
            for (int e = 1; e < i + 1; e++)
            {
                int[] combination = new int[e];
                for (int k = 0; k < e; k++)
                    combination[k] = k;
                bool existenceNextPermutation = true;
                while (existenceNextPermutation)
                {
                    int x = 1;
                    for (int k = 0; k < e; k++)
                        x *= v[combination[k]];
                    int c = y / x;
                    if (x != 1)
                        if (arrayOfRes[x] + arrayOfRes[c] > res)
                            res = arrayOfRes[x] + arrayOfRes[c];
                    existenceNextPermutation = FindNextPermutation(e, i, ref combination);
                }
            }
            arrayOfRes[y] = (byte)res;
            return res;
        }

        private int FindNextDivider(ref int y)
        {
            if (y % 2 == 0)
            {
                y /= 2;
                return 2;
            }
            int x = 3;
            while (x * x <= y)
            {
                if (y % x == 0)
                {
                    y /= x;
                    return x;
                }
                x += 2;
            }
            return 1;
        }

        private bool FindNextPermutation(int k, int n, ref int[] digits)
        {
            int i;
            int overflow;
            for (i = k - 1, overflow = n; i != -1;)
            {
                digits[i]++;
                if (digits[i] < overflow)
                    break;
                i--;
                overflow--;
            }
            if (i != -1)
            {
                for (i = i + 1; i < k; i++)
                    digits[i] = digits[i - 1] + 1;
                return true;
            }
            return false;
        }
    }
}
