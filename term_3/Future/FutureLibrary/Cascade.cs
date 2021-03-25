using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureLibrary
{
    public class Cascade : IVectorLengthComputer
    {
        public double ComputeLength(int[] coordinates)
        {
            List<Task<int>> coordinatesComputer = new List<Task<int>>();
            for (int i = 0; i < coordinates.Length; ++i)
            {
                int number = coordinates[i];
                //Console.WriteLine($"Squeared {number}");
                coordinatesComputer.Add(Task.Factory.StartNew
                    (() => number * number));
            }
            while (coordinatesComputer.Count > 1)
            {
                List<Task<int>> tempIterration = new List<Task<int>>();
                if (coordinatesComputer.Count % 2 != 0)
                {
                    coordinatesComputer.Add(Task.Factory.StartNew(() => 0));
                }
                for (int i = 0; i < coordinatesComputer.Count; i +=2)
                {
                    int coord = coordinatesComputer[i + 1].Result;
                    tempIterration.Add(coordinatesComputer[i].ContinueWith((self) => self.Result + coord));
                }
                foreach (var task in coordinatesComputer)
                {
                    task.Wait();
                    task.Dispose();
                }
                coordinatesComputer = tempIterration;
            }
            return Math.Sqrt(coordinatesComputer[0].Result);
        }
    }
}
