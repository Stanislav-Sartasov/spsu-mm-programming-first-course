using System;

namespace Future
{
    class Program
    {
        static void Main(string[] args)
        {
            var vector = ArrayGenerator.GenerateArray(1000000);

            IVectorLengthComputer[] vectorLengthComputers = new IVectorLengthComputer[] { new CascadeModel(), new ModifiedCascadeModel(), new SequentialModel() };
            for (int i = 0; i < vectorLengthComputers.Length; i++)
                Console.WriteLine($"{vectorLengthComputers[i]}: {vectorLengthComputers[i].ComputeLength(vector)}");         
        }
    }
}
