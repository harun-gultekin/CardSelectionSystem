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

## Card Visual Spec
Face-down card:
- Uses sprites from Sprites/Cards/ folder
- Border color: #69748c (gray)

Face-up card:
- Displays: item sprite + item name text + colored background + colored border
- Background AND border use the item's tier hex color
- Reference image: Sprites/CardRef.png

## Provided Assets
- Sprites/Cards/ → card back/frame sprites
- Sprites/Items/ → individual item sprites (may contain extra files beyond the 10 listed)
- Sprites/CardRef.png → reference showing expected face-up card appearance

## ScriptableObject Structure
```csharp
[CreateAssetMenu(fileName = "ItemData", menuName = "Game/Item Data")]
public class ItemData : ScriptableObject {
    public string itemName;
    public string abbreviation;  // for debug panel
    public Sprite itemSprite;
    public Color tierColor;       // parsed from hex
    public CardTier tier;
    public int cardsPerCycle;
}
```

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
