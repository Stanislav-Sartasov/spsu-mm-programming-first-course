using Interfaces;

namespace FirstPlugin
{
    public class FibonacciPlugin : IFibonacci
    {
        public int GetFibonacci(int num)
        {
            int x = 0;
            int y = 1;
            for (int i = 0; i < num; i++)
            {
                y += x;
                x = y - x;
            }
            return x;
        }
    }
}
