namespace CardSelectionSystem.Core.Models
{
    public class ItemConfig
    {
        public string Name { get; }
        public int CardsPerCycle { get; }
        public CardTier Tier { get; }
        public string ColorHex { get; }
        public string Abbreviation { get; }

        public ItemConfig(string name, int cardsPerCycle, CardTier tier, string colorHex, string abbreviation)
        {
            Name = name;
            CardsPerCycle = cardsPerCycle;
            Tier = tier;
            ColorHex = colorHex;
            Abbreviation = abbreviation;
        }
    }
}
