using System.Collections.Generic;
using System.Linq;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Core.Distribution
{
    public class DistributionValidator : IDistributionValidator
    {
        private readonly BlockCalculator _blockCalculator;

        public DistributionValidator(BlockCalculator blockCalculator)
        {
            _blockCalculator = blockCalculator;
        }

        public ValidationResult Validate(string[] sequence, List<ItemConfig> items, int totalPositions)
        {
            var errors = new List<string>();
            int validCount = 0;
            int totalCount = totalPositions;

            // Check 1: sequence length
            if (sequence == null || sequence.Length != totalPositions)
            {
                errors.Add($"Sequence length is {sequence?.Length ?? 0}, expected {totalPositions}.");
                return new ValidationResult(false, 0, totalCount, errors);
            }

            // Check 2: each item appears exactly cardsPerCycle times
            var actualCounts = new Dictionary<string, int>();
            foreach (string name in sequence)
            {
                if (!actualCounts.ContainsKey(name))
                    actualCounts[name] = 0;
                actualCounts[name]++;
            }

            foreach (var item in items)
            {
                int actual = actualCounts.ContainsKey(item.Name) ? actualCounts[item.Name] : 0;
                if (actual != item.CardsPerCycle)
                {
                    errors.Add($"{item.Name}: expected {item.CardsPerCycle} occurrences, found {actual}.");
                }
            }

            // Check for unexpected items in the sequence
            var expectedNames = new HashSet<string>(items.Select(i => i.Name));
            foreach (var kvp in actualCounts)
            {
                if (!expectedNames.Contains(kvp.Key))
                {
                    errors.Add($"Unexpected item '{kvp.Key}' found in sequence.");
                }
            }

            if (errors.Count > 0)
            {
                return new ValidationResult(false, 0, totalCount, errors);
            }

            // Check 3: for each item, verify the i-th occurrence is within block[i]
            foreach (var item in items)
            {
                var blocks = _blockCalculator.CalculateBlocks(item.CardsPerCycle, totalPositions);

                // Find all positions of this item, sorted ascending
                var positions = new List<int>();
                for (int pos = 0; pos < sequence.Length; pos++)
                {
                    if (sequence[pos] == item.Name)
                        positions.Add(pos);
                }

                // The i-th occurrence (by position order) maps to block[i]
                for (int i = 0; i < positions.Count; i++)
                {
                    int pos = positions[i];
                    var block = blocks[i];

                    if (pos >= block.Start && pos <= block.End)
                    {
                        validCount++;
                    }
                    else
                    {
                        errors.Add($"{item.Name} instance {i}: position {pos} is outside block [{block.Start}-{block.End}].");
                    }
                }
            }

            bool isValid = errors.Count == 0;
            return new ValidationResult(isValid, validCount, totalCount, errors);
        }
    }
}
