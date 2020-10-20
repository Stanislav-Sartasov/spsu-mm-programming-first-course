using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Future
{
    public class ModifiedCascadeModel : IVectorLengthComputer
    {
        public double ComputeLength(int[] a)
        {
            var vector = a.ToList();
            if (vector.Count % 2 == 1)
                vector.Add(0);
            var dim = vector.Count();
            var seqBlocksNumber = (int)Math.Log(dim, 2);
            var blocks = SplitIntoBlocks(vector, seqBlocksNumber);
            var tasks = new List<Task<int>>();
            foreach (var block in blocks)
                tasks.Add(Task.Factory.StartNew(() => block.Sum(Square)));
            while (true)
            {
                var resultsPairs = tasks.Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / 2)
                    .Select(grp => grp.Select(x => x.Value).ToList())
                    .ToList();
                tasks.Clear();
                int count = resultsPairs.Count();
                if (count == 1)
                    return Math.Sqrt(resultsPairs.Sum(pair => pair.Sum(task => task.Result)));
                else if (resultsPairs.Last().Count % 2 == 1)
                {
                    tasks.AddRange(resultsPairs.Last());
                    resultsPairs.RemoveAt(count - 1);
                }
                tasks.AddRange(resultsPairs.Select(pair =>
                Task.Factory.StartNew(() => Sum(pair[0].Result, pair[1].Result))));
            }

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

        private int Sum(int x, int y) => x + y;

        private int Square(int x) => x * x;
    }
}
