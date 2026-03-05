using System.Collections.Generic;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Core.Distribution
{
    public interface IDistributor
    {
        string[] Generate(List<ItemConfig> items, int totalPositions = 50);
    }
}
