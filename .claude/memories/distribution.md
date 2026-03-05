# Distribution Algorithm

## Block Calculation
For item with count = N:
  blockSize = 50.0 / N
  block[i].start = floor(i * blockSize)
  block[i].end = floor((i+1) * blockSize) - 1

## Placement Strategy
1. Create all item-instance placements → 50 total
2. Sort by block width ascending (most constrained first)
3. Backtracking with shuffle for randomness
4. Validate after generation

## Item Pool (total = 50)
Headshot:2, Sniper:3, Vampire:4, Titan:5, Chain:5,
Pierce:5, RapidFire:5, Evasion:7, SplashDamage:7, Shield:7
