using System.Collections.Generic;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Core.Distribution
{
    public interface IDistributionValidator
    {
        ValidationResult Validate(string[] sequence, List<ItemConfig> items, int totalPositions);
    }
}
