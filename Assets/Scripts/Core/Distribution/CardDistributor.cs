using System;
using System.Collections.Generic;
using System.Linq;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Core.Distribution
{
    public class CardDistributor : IDistributor
    {
        private const int MaxIterationsPerAttempt = 100_000;
        private const int MaxAttempts = 10;

        private readonly BlockCalculator _blockCalculator;
        private readonly Random _random;
        private int _iterationCount;

        public CardDistributor(BlockCalculator blockCalculator, int? seed = null)
        {
            _blockCalculator = blockCalculator;
            _random = seed.HasValue ? new Random(seed.Value) : new Random();
        }

        public string[] Generate(List<ItemConfig> items, int totalPositions = 50)
        {
            if (!IsConfigurationPossible(items, totalPositions))
                return null;

            var placements = BuildPlacements(items, totalPositions);

            for (int attempt = 0; attempt < MaxAttempts; attempt++)
            {
                var shuffled = SortAndShufflePlacements(placements);
                var positions = new string[totalPositions];
                _iterationCount = 0;

                if (Place(positions, shuffled, 0))
                    return positions;
            }

            return null;
        }

        public bool IsConfigurationPossible(List<ItemConfig> items, int totalPositions)
        {
            int totalCount = items.Sum(item => item.CardsPerCycle);
            if (totalCount != totalPositions)
                return false;

            // Pigeonhole check: for each position, count how many blocks claim it.
            // If any position is claimed by more items than can fit, it's impossible
            // only if the total demand across all positions in a region exceeds capacity.
            // Simple check: for each position, count overlapping blocks.
            var positionDemand = new int[totalPositions];

            foreach (var item in items)
            {
                var blocks = _blockCalculator.CalculateBlocks(item.CardsPerCycle, totalPositions);
                foreach (var block in blocks)
                {
                    for (int pos = block.Start; pos <= block.End; pos++)
                    {
                        positionDemand[pos]++;
                    }
                }
            }

            // Check contiguous regions where demand exceeds supply
            // For a more thorough check: any single position can only hold 1 item,
            // so we check sliding windows where demand minus capacity is impossible.
            // A simple necessary condition: for any range [a,b], the number of blocks
            // fully contained in [a,b] must not exceed (b - a + 1).
            // We approximate with per-position check as a fast heuristic.
            for (int pos = 0; pos < totalPositions; pos++)
            {
                if (positionDemand[pos] <= 0)
                    return false; // No item can go here
            }

            return true;
        }

        private List<Placement> BuildPlacements(List<ItemConfig> items, int totalPositions)
        {
            var placements = new List<Placement>();

            foreach (var item in items)
            {
                var blocks = _blockCalculator.CalculateBlocks(item.CardsPerCycle, totalPositions);
                for (int i = 0; i < blocks.Count; i++)
                {
                    placements.Add(new Placement(item.Name, blocks[i].Start, blocks[i].End));
                }
            }

            return placements;
        }

        private List<Placement> SortAndShufflePlacements(List<Placement> placements)
        {
            // Step 1: Stable sort by block width ascending
            var sorted = placements.OrderBy(p => p.BlockWidth).ToList();

            // Step 2: Shuffle within each same-width group
            int groupStart = 0;
            while (groupStart < sorted.Count)
            {
                int groupWidth = sorted[groupStart].BlockWidth;
                int groupEnd = groupStart;
                while (groupEnd < sorted.Count && sorted[groupEnd].BlockWidth == groupWidth)
                    groupEnd++;

                // Shuffle the range [groupStart, groupEnd)
                for (int i = groupEnd - 1; i > groupStart; i--)
                {
                    int j = _random.Next(groupStart, i + 1);
                    var temp = sorted[i];
                    sorted[i] = sorted[j];
                    sorted[j] = temp;
                }

                groupStart = groupEnd;
            }

            return sorted;
        }

        private bool Place(string[] positions, List<Placement> placements, int index)
        {
            _iterationCount++;
            if (_iterationCount > MaxIterationsPerAttempt)
                return false;

            if (index == placements.Count)
                return true;

            var placement = placements[index];

            // Gather candidate positions (empty slots within this block)
            var candidates = new List<int>();
            for (int pos = placement.BlockStart; pos <= placement.BlockEnd; pos++)
            {
                if (positions[pos] == null)
                    candidates.Add(pos);
            }

            // Shuffle for randomness
            Shuffle(candidates);

            foreach (int pos in candidates)
            {
                positions[pos] = placement.ItemName;

                if (Place(positions, placements, index + 1))
                    return true;

                positions[pos] = null;
            }

            return false;
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private readonly struct Placement
        {
            public string ItemName { get; }
            public int BlockStart { get; }
            public int BlockEnd { get; }
            public int BlockWidth => BlockEnd - BlockStart + 1;

            public Placement(string itemName, int blockStart, int blockEnd)
            {
                ItemName = itemName;
                BlockStart = blockStart;
                BlockEnd = blockEnd;
            }
        }
    }
}
