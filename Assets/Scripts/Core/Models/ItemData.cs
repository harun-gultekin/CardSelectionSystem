using UnityEngine;

namespace CardSelectionSystem.Core.Models
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Game/Item Data")]
    public class ItemData : ScriptableObject
    {
        public string itemName;
        public int cardsPerCycle;
        public CardTier tier;
        public Color tierColor;
        public string colorHex;
        public string abbreviation;
        public Sprite itemSprite;

        public ItemConfig ToItemConfig()
        {
            return new ItemConfig(itemName, cardsPerCycle, tier, colorHex, abbreviation);
        }
    }
}
