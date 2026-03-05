# Distribution Algorithm

## Evaluation Criteria (from case doc)
The company evaluates distribution FIRST and asks:
- Does it ALWAYS produce a valid distribution?
- Are ALL item-instances placed within designated blocks?
- Does it handle COMPETITION between overlapping blocks correctly?
- Can it DETECT when a configuration is impossible?
- Unit tests are a plus.

## Block Calculation Formula
For item with count = N, totalPositions = 50:
```
blockSize = 50.0 / N   (FLOAT division, not integer)
block[i].start = floor(i * blockSize)
block[i].end   = floor((i + 1) * blockSize) - 1
```

IMPORTANT: Blocks can have UNEVEN widths. For N=7:
- blockSize = 50/7 = 7.142857...
- Block 0: floor(0) to floor(7.14)-1 = [0, 6] → width 7
- Block 1: floor(7.14) to floor(14.28)-1 = [7, 13] → width 7
- Block 6: floor(42.85) to floor(50)-1 = [42, 49] → width 8
Some blocks are 7 wide, some are 8 wide.

## Block Table (all 10 items)
| Item | Count | Block Ranges |
|------|-------|-------------|
| Headshot | 2 | [0-24], [25-49] |
| Sniper | 3 | [0-15], [16-32], [33-49] |
| Vampire | 4 | [0-11], [12-24], [25-36], [37-49] |
| Titan | 5 | [0-9], [10-19], [20-29], [30-39], [40-49] |
| Chain | 5 | [0-9], [10-19], [20-29], [30-39], [40-49] |
| Pierce | 5 | [0-9], [10-19], [20-29], [30-39], [40-49] |
| RapidFire | 5 | [0-9], [10-19], [20-29], [30-39], [40-49] |
| Evasion | 7 | [0-6], [7-13], [14-20], [21-27], [28-34], [35-41], [42-49] |
| SplashDmg | 7 | [0-6], [7-13], [14-20], [21-27], [28-34], [35-41], [42-49] |
| Shield | 7 | [0-6], [7-13], [14-20], [21-27], [28-34], [35-41], [42-49] |

## Overlap Problem
Position 0 is claimed by ALL 10 items' first blocks.
Position 5 is inside blocks of: Shield[0-6], Evasion[0-6], SplashDmg[0-6],
Chain[0-9], Pierce[0-9], RapidFire[0-9], Titan[0-9], Vampire[0-11],
Sniper[0-15], Headshot[0-24] — 10 items want it, only 1 can have it.

## Placement Strategy — Most Constrained First + Backtracking

### Step 1: Create placement list
For each item, for each instance → (itemName, blockStart, blockEnd)
Total: 50 placements

### Step 2: Sort by constraint level
Sort placements by block width ASCENDING (narrowest blocks first).
- Shield/Evasion/SplashDmg blocks (~7 positions) → placed FIRST
- Headshot blocks (25 positions) → placed LAST
Ties can be broken randomly or by item order.

### Step 3: Backtracking placement
```
function Place(positions[], placements[], index):
    if index == 50 → return true (all placed!)
    
    placement = placements[index]
    candidates = empty positions within [placement.start, placement.end]
    shuffle(candidates)  // randomness
    
    for each pos in candidates:
        positions[pos] = placement.itemName
        if Place(positions, placements, index + 1):
            return true
        positions[pos] = null  // BACKTRACK
    
    return false  // no candidate worked, backtrack further up
```

### Step 4: Impossible detection
If Place() returns false after exhausting all options → configuration is impossible.
Also pre-validate: sum of all item counts must equal totalPositions (50).
Also check: no single position is demanded by more items than available positions
in overlapping blocks (pigeonhole principle check).

## Validation (DistributionValidator)
After generation, verify:
1. Sequence length == 50
2. Each item appears exactly cardsPerCycle times
3. For each item: sort its positions ascending, then check position[i] is
   within block[i].start and block[i].end
   IMPORTANT: positions must be checked IN ORDER against blocks IN ORDER.
   The i-th occurrence (by position) maps to the i-th block.

## Item Pool (total = 50)
Headshot:2, Sniper:3, Vampire:4, Titan:5, Chain:5,
Pierce:5, RapidFire:5, Evasion:7, SplashDamage:7, Shield:7

## Unit Test Cases to Cover
- Block calculation for 2, 3, 4, 5, 7 card items
- Blocks cover full 0-49 range with no gaps
- Generate produces exactly 50 cards
- Each item appears correct count
- All instances within designated blocks (validated in order)
- 100 consecutive runs all produce valid sequences
- Two runs produce different sequences (randomness)
- Impossible config detected (e.g., 51 total cards, or extreme overlap)
- Single item with count=50 (edge case)
- Single item with count=1 (edge case)
