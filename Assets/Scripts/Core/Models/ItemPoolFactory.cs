using System.Collections.Generic;

namespace CardSelectionSystem.Core.Models
{
    public static class ItemPoolFactory
    {
        public static List<ItemConfig> CreateDefaultPool()
        {
            return new List<ItemConfig>
            {
                new ItemConfig("Headshot",      2, CardTier.Gold,   "#fcc325", "Hs"),
                new ItemConfig("Sniper",        3, CardTier.Gold,   "#fcc325", "Sn"),
                new ItemConfig("Vampire",       4, CardTier.Purple, "#ba26ff", "Va"),
                new ItemConfig("Titan",         5, CardTier.Purple, "#ba26ff", "Ti"),
                new ItemConfig("Chain",         5, CardTier.Blue,   "#008aff", "Ch"),
                new ItemConfig("Pierce",        5, CardTier.Blue,   "#008aff", "Pi"),
                new ItemConfig("Rapid Fire",    5, CardTier.Blue,   "#008aff", "Rf"),
                new ItemConfig("Evasion",       7, CardTier.Green,  "#5cb85c", "Ev"),
                new ItemConfig("Splash Damage", 7, CardTier.Green,  "#5cb85c", "Sp"),
                new ItemConfig("Shield",        7, CardTier.Green,  "#5cb85c", "Sh"),
            };
        }
    }
}
