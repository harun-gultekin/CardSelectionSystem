using System;
using System.Collections.Generic;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Core.Distribution
{
    public class BlockCalculator
    {
        public List<BlockRange> CalculateBlocks(int itemCount, int totalPositions)
        {
            if (itemCount <= 0)
                throw new ArgumentException("Item count must be positive.", nameof(itemCount));
            if (totalPositions <= 0)
                throw new ArgumentException("Total positions must be positive.", nameof(totalPositions));
            if (itemCount > totalPositions)
                throw new ArgumentException("Item count cannot exceed total positions.");

            var blocks = new List<BlockRange>(itemCount);
            float blockSize = (float)totalPositions / itemCount;

            for (int i = 0; i < itemCount; i++)
            {
                int start = (int)Math.Floor(i * blockSize);
                int end = (int)Math.Floor((i + 1) * blockSize) - 1;
                blocks.Add(new BlockRange(start, end));
            }

            return blocks;
        }
    }
}
