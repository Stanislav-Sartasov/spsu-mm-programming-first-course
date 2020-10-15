using Extensions;
using MPI;
using System;
using System.Collections.Generic;
using System.Linq;
using Environment = MPI.Environment;

class MPIQuickSort
{
    static List<Tuple<int, int>> GetBlockPairs(int step, int blocksNumber, int stepsNumber)
    {
        int bitPosition = stepsNumber - step - 1;
        List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
        List<int> remainingNumbers = Enumerable.Range(0, blocksNumber).ToList();
        while (remainingNumbers.Count() > 0)
        {
            var num = remainingNumbers.First();
            var assocNum = num + (1 << bitPosition);
            pairs.Add(Tuple.Create(num, assocNum));
            remainingNumbers.Remove(num);
            remainingNumbers.Remove(assocNum);
        }
        return pairs;
    }

    static int GetNodeNumber(Tuple<int, int> pair, int step, int stepsNumber)
    {
        int bitPosition = stepsNumber - step - 1;
        var num = pair.Item1;
        return (num >> (bitPosition + 1) << bitPosition) + num % (1 << bitPosition);
    }

    static List<Block> SplitIntoBlocks(List<int> data, int blocksNumber)
    {
        var size = data.Count;
        var blocks = data
            .Select((s, i) => new { Value = s, Index = i })
            .GroupBy(x => x.Index / (size / blocksNumber))
            .Select((grp, i) => new Block(i, grp.Select(x => x.Value).ToList()))
            .ToList();
        if (blocks.Count > blocksNumber)
        {
            var extraBlock = blocks.Last();
            blocks.RemoveAt(blocksNumber);
            int blockNumber = 0;
            foreach (var e in extraBlock.Data)
            {
                blocks[blockNumber++ % blocksNumber].Data.Add(e);
            }
        }
        else if (blocks.Count < blocksNumber)
        {
            while (blocks.Count != blocksNumber)
                blocks.Add(new Block(blocks.Count, new List<int>()));
        }

        return blocks;
    }

    static Dictionary<int, Tuple<int, int>> GetPairsMap(List<Tuple<int, int>> pairs, int stepsNumber, int step)
    {
        var map = new Dictionary<int, Tuple<int, int>>();

        foreach (var p in pairs)
        {
            int rank = GetNodeNumber(p, step, stepsNumber);
            map.Add(rank, p);
        }

        return map;
    }

    public static List<List<int>> MergeSplit(List<int> array, int pivot)
    {
        if (array.Count() == 0)
            return new List<List<int>>() { new List<int>(), new List<int>() };
        var groups = array
                        .GroupBy(x => x < pivot)
                        .Select(grp => grp.ToList())
                        .ToList();
        return groups;
    }

    public static int CalculatePivot(List<int> group, List<Block> blocks)
    {
        var r = new Random(DateTime.Now.Second);
        var pivot = (int)blocks
            .Where(b => group.Contains(b.Number) && b.Data.Count > 0)
            .Select(b => b.Data[r.Next(b.Data.Count)]).Average();
        return pivot;
    }

    public static bool CheckInput(string[] args, int processorsCount)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("\nInvalid number of argumets!");
            Console.WriteLine("args: <input path> <output path>");
            Console.WriteLine("Try again!");
            return false;
        }

        if (!processorsCount.IsPowerOfTwo())
        {
            Console.WriteLine("\nThe number of processors must be a power of two! Try again!");
            return false;
        }

        return true;
    }

    static void Main(string[] args)
    {
        using (new Environment(ref args))
        {
            var world = Communicator.world;
            var processors = world.Size;
            var blocksNumber = 2 * processors;
            List<int> input = null;
            List<Block> blocks = null;
            var stepsNumber = (int)Math.Log(blocksNumber, 2);

            if (world.Rank == 0)
            {

                if (!CheckInput(args, processors))
                {
                    world.Abort(-1);
                }

                try
                {
                    input = IOManager.ReadArray(args[0]);
                }
                catch
                {
                    Console.WriteLine($"\nUnable to read file: {args[0]}");
                    world.Abort(-2);
                }

                blocks = SplitIntoBlocks(input, blocksNumber);
            }


            List<List<int>> pivotGroups = new List<List<int>>()
            {
                Enumerable.Range(0, blocksNumber).ToList()
            };

            for (int i = 0; i < stepsNumber; i++)
            {
                world.Barrier();

                if (world.Rank == 0)
                {
                    var pairs = GetBlockPairs(i, blocksNumber, stepsNumber);
                    var map = GetPairsMap(pairs, stepsNumber, i);
                    var received = new List<Block>();
                    var pivotsMap = new Dictionary<List<int>, int>();

                    foreach (var grp in pivotGroups)
                    {
                        pivotsMap.Add(grp, CalculatePivot(grp, blocks));
                    }

                    Tuple<int, int> blockPair;
                    List<int> data;

                    for (int proc = 0; proc < processors; proc++)
                    {
                        blockPair = map[proc];
                        data = Enumerable
                            .Concat(blocks[blockPair.Min()].Data, blocks[blockPair.Max()].Data)
                            .ToList();
                        if (proc == 0)
                        {
                            var groups = MergeSplit(data, pivotsMap[pivotGroups.Where(p => p.Contains(blockPair.Item1)).First()]);
                            if (groups.Count == 1)
                                groups.Add(new List<int>());
                            received
                                .Add(new Block(blockPair.Min(), groups[0].FirstOrDefault() < groups[1].FirstOrDefault() ?
                                groups[0] : groups[1]));
                            received
                                .Add(new Block(blockPair.Max(), groups[0].FirstOrDefault() > groups[1].FirstOrDefault() ?
                                groups[0] : groups[1]));
                        }
                        else
                        {
                            world.Send(data, proc, 0);
                            world.Send(pivotsMap[pivotGroups.Where(p => p.Contains(blockPair.Item1)).First()], proc, 1);
                        }
                    }

                    for (int proc = 1; proc < processors; proc++)
                    {
                        var pair = map[proc];
                        var data1 = world.Receive<List<int>>(proc, 0);
                        var data2 = world.Receive<List<int>>(proc, 1);
                        received
                            .Add(new Block(pair.Min(), data1.FirstOrDefault() < data2.FirstOrDefault() ?
                            data1 : data2));
                        received
                            .Add(new Block(pair.Max(), data1.FirstOrDefault() > data2.FirstOrDefault() ?
                            data1 : data2));
                    }

                    received.Sort();
                    blocks = received;
                    pivotGroups = pivotGroups
                        .SelectMany(g => g.GroupBy(e => e < g.Average()).Select(gr => gr.ToList()))
                        .ToList();
                }

                else
                {
                    var batch = world.Receive<List<int>>(0, 0);
                    var pivot = world.Receive<int>(0, 1);
                    var groups = MergeSplit(batch, pivot);
                    if (groups.Any())
                        world.Send(groups[0], 0, 0);
                    if (groups.Count() == 2)
                        world.Send(groups[1], 0, 1);
                    else
                        world.Send(new List<int>(), 0, 1);

                }
            }

            if (world.Rank == 0)
            {
                //local sorting
                blocks.ForEach(b => b.Data.Sort());
                var result = blocks.SelectMany(b => b.Data).ToList();
                try
                {
                    IOManager.WriteArray(args[1], result);
                }
                catch
                {
                    Console.WriteLine($"\nUnable to write in file: {args[1]}");
                    world.Abort(-3);
                }
                Console.WriteLine("Done");
            }
        }
    }
}