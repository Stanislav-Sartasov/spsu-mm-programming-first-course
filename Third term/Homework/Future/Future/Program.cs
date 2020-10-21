using System;

namespace Future
{
    class Program
    {
        static void Main(string[] args)
        {
            var vector = ArrayGenerator.GenerateArray(10000);

            IVectorLengthComputer[] vectorLengthComputers = new IVectorLengthComputer[] 
            { 
                new CascadeModel(), 
                new ModifiedCascadeModel(), 
                new SequentialModel() 
            };
            for (int i = 0; i < vectorLengthComputers.Length; i++)
            {
                var res = vectorLengthComputers[i].ComputeLength(vector);
                Console.WriteLine($"{vectorLengthComputers[i]}: {res}");
            }
        }
    }
}
