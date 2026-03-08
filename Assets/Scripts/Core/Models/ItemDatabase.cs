using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardSelectionSystem.Core.Models
{
    [CreateAssetMenu(fileName = "ItemDatabase", menuName = "Game/Item Database")]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemData> items;

        public List<ItemConfig> ToItemConfigList()
        {
            return items.Select(i => i.ToItemConfig()).ToList();
        }

        public Dictionary<string, Sprite> BuildSpriteDictionary()
        {
            return items.ToDictionary(i => i.itemName, i => i.itemSprite);
        }
    }
}
