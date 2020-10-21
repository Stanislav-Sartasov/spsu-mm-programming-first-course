using FutureLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Future
{
    public class ModifiedCascadeModel : CascadeBase, IVectorLengthComputer
    {
        public double ComputeLength(int[] a)
        {
            double result;
            try
            {
                var vector = a.ToList();
                if (vector.Count % 2 == 1)
                    vector.Add(0);
                var dim = vector.Count();
                var seqBlocksNumber = Environment.ProcessorCount / 2;
                var blocks = SplitIntoBlocks(vector, seqBlocksNumber);
                var tasks = new List<Task<int>>();
                foreach (var block in blocks)
                    tasks.Add(Task.Factory.StartNew(() => block.Sum(Square)));
                result = CascadeCalculate(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                result = double.NaN;
            }
            return result;
        }

        static List<List<int>> SplitIntoBlocks(List<int> data, int blocksNumber)
        {
            var size = data.Count;
            var blocks = data
                .Select((s, i) => new { Value = s, Index = i })
                .GroupBy(x => x.Index / (size / blocksNumber))
                .Select(grp => grp.Select(x => x.Value).ToList())
                .ToList();
            if (blocks.Count > blocksNumber)
            {
                var extraBlock = blocks.Last();
                blocks.RemoveAt(blocksNumber);
                int blockNumber = 0;
                foreach (var e in extraBlock)
                {
                    blocks[blockNumber++ % blocksNumber].Add(e);
                }
            }
            return blocks;
        }

        private int Square(int x) => x * x;
    }
}
