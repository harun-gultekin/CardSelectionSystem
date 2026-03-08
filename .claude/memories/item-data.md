# Item Database

## Item Pool (total = 50 cards per cycle)
| Item Name | Sprite File | Count | Hex | Tier | Debug Abbrev |
|-----------|-------------|-------|-----|------|-------------|
| Headshot | item_headshot.png | 2 | #fcc325 | Gold | Hs |
| Sniper | item_sniper.png | 3 | #fcc325 | Gold | Sn |
| Vampire | item_vampire.png | 4 | #ba26ff | Purple | Va |
| Titan | item_titan.png | 5 | #ba26ff | Purple | Ti |
| Chain | item_chain.png | 5 | #008aff | Blue | Ch |
| Pierce | item_pierce.png | 5 | #008aff | Blue | Pi |
| Rapid Fire | item_rapid_fire.png | 5 | #008aff | Blue | Rf |
| Evasion | item_evasion.png | 7 | #5cb85c | Green | Ev |
| Splash Damage | item_splash_damage.png | 7 | #5cb85c | Green | Sp |
| Shield | item_shield_orbital.png | 7 | #5cb85c | Green | Sh |

## Card Visual Structure (multi-layered sprites)
The card is NOT a single sprite. It's a parent GameObject with layered children.
### Sprite Assets (Sprites/Cards/):
- border.png (128x128, 9-slice) — outer card frame
- card.png (128x128, 9-slice) — card background fill
- header.png (90x90, 9-slice) — name banner at top of face-up card
- itembg.png (110x110, 9-slice) — rounded rect behind item sprite
- logo.png (290x252, normal) — mascot shown on face-down card
### Card Hierarchy:
Card (parent, BoxCollider2D, CardView script)
  ├── Border (SpriteRenderer, sorting order 0)
  ├── CardBg (SpriteRenderer, sorting order 1)
  ├── Logo (SpriteRenderer, sorting order 2, face-down only)
  ├── Header (SpriteRenderer, sorting order 2, face-up only)
  ├── ItemBg (SpriteRenderer, sorting order 2, face-up only)
  ├── ItemSprite (SpriteRenderer, sorting order 3, face-up only)
  └── ItemName (TextMeshPro world space, sorting order 4, face-up only)
### Face-down state:
- border: visible, color #69748c (gray)
- card: visible, color #69748c
- logo: visible
- header, itemBg, itemSprite, itemName: HIDDEN
### Face-up state:
- border: visible, color = tier hex color
- card: visible, color = tier hex color
- logo: HIDDEN
- header: visible, color white
- itemBg: visible, color white
- itemSprite: visible, sprite = item sprite
- itemName: visible, text = item name
### Item sprites (Sprites/Items/):
All 260x260, normal (not 9-slice).

## ScriptableObject Structure
```csharp
[CreateAssetMenu(fileName = "ItemData", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject {
    public string itemName;
    public int cardsPerCycle;
    public CardTier tier;
    public Color tierColor;
    public string colorHex;
    public string abbreviation;
    public Sprite itemSprite;

    public ItemConfig ToItemConfig() {
        return new ItemConfig(itemName, cardsPerCycle, tier, colorHex, abbreviation);
    }
}

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Game/Item Database")]
public class ItemDatabase : ScriptableObject {
    public List<ItemData> items;

    public List<ItemConfig> ToItemConfigList()
        => items.Select(i => i.ToItemConfig()).ToList();

    public Dictionary<string, Sprite> BuildSpriteDictionary()
        => items.ToDictionary(i => i.itemName, i => i.itemSprite);
}
```

ItemPoolFactory remains for unit tests. Runtime uses ItemDatabase
ScriptableObject which is configured entirely in Inspector.

## CardTier Enum
```csharp
public enum CardTier { Green, Blue, Purple, Gold }
```

## ItemConfig (pure C# for algorithm, no Unity dependency)
```csharp
public class ItemConfig {
    public string Name;
    public int CardsPerCycle;
    public CardTier Tier;
    public string ColorHex;
    public string Abbreviation;
}
```
