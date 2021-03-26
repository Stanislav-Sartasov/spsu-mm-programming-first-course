using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutureLibrary
{
    public class ModifiedCascade: IVectorLengthComputer
    {
        public double ComputeLength(int[] coordinates)
        {
            List<Task<int>> coordinatesComputer = new List<Task<int>>();
            int processors = (int)(coordinates.Length * Math.Log(2, coordinates.Length));
            //WHAT??? Math.Log(2, coordinates.Length) returns 1/Log2(N). How can it be?? have i miss arguments place??
            //Console.WriteLine($"calc: length: {coordinates.Length}, log2N: {1/Math.Log(2, coordinates.Length)}");
            for (int i = 0; i < processors; ++i)
            {
                int[] temp = new int[coordinates.Length/processors + (i == processors - 1? coordinates.Length % processors:0)];
                for (int j = 0; j < temp.Length; ++j)
                {
                    temp[j] = coordinates[i * (coordinates.Length / processors) + j];
                    //Console.Write($"{temp[j]}");
                }
                //Console.WriteLine($" for process {i}; processor total: {processors}; length: {temp.Length}");
                coordinatesComputer.Add(Task.Factory.StartNew
                    (() => 
                    {
                        int sum = 0;
                        //string str = "";
                        foreach (var number in temp)
                        {
                            //str = $" {str},  {number.ToString()}";
                            sum += number * number;
                        }
                        //Console.WriteLine($"\ntask {Task.CurrentId} array is {str};; sum is {sum}");
                        return sum;    
                    }));
            }
            while (coordinatesComputer.Count > 1)
            {
                List<Task<int>> tempIterration = new List<Task<int>>();
                if (coordinatesComputer.Count % 2 != 0)
                {
                    coordinatesComputer.Add(Task.Factory.StartNew(() => 0));
                }
                for (int i = 0; i < coordinatesComputer.Count; i += 2)
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
