# Animation System

## Hard Rules (from case doc)
- Unity Animator/Animation component FORBIDDEN for card animations
- Code-driven only: DOTween, PrimeTween, LeanTween, or manual interpolation
- At least 3 visually DISTINCT easing curves across animations
- All duration values must be [SerializeField] editable in Inspector
- Do NOT hardcode any animation parameter values

## AnimationConfig — [Serializable] class with [SerializeField] fields
| Parameter | Field Name | Default | Description |
|-----------|-----------|---------|-------------|
| Deal duration | dealDuration | 0.4s | Card slides into position |
| Green flip | greenFlipDuration | 0.3s | Quick and snappy |
| Blue flip | blueFlipDuration | 0.5s | Moderate pace |
| Purple flip | purpleFlipDuration | 0.8s | With suspense pause |
| Purple pause | purpleMidpointPause | 0.15s | Hold at zero-width |
| Gold flip | goldFlipDuration | 1.2s | Dramatic reveal |
| Gold pause | goldMidpointPause | 0.3s | Longer hold at zero-width |
| Discard duration | discardDuration | 0.3s | Card slides off-screen |

## Easing Curves (minimum 3 distinct required)
1. Deal → EaseOutCubic — decelerates (fast start, gentle settle)
2. Flip compress (first half) → EaseInQuad — accelerates (slow start, speeds up)
3. Flip expand (second half) → EaseOutQuad — decelerates (fast start, gentle settle)
4. Discard → EaseInCubic — accelerates (slow start, fast exit)

## Animation Flows

### 1. DEAL — Card enters screen
- Card starts BELOW visible screen area, face-down
- Slides UPWARD to center of screen
- Movement DECELERATES (EaseOutCubic)
- Duration: dealDuration (default 0.4s)

### 2. FLIP — Card reveals (tier-dependent)

**Phase 1 — Compress:**
- localScale.x: 1 → 0 (EaseInQuad)
- Duration: flipDuration / 2
- Visually: card narrows as if turning sideways

**Midpoint — Swap:**
- At scale.x == 0: call CardView.ShowFaceUp() which:
  - Hides logo
  - Shows header (white), itemBg (white), itemSprite, itemName
  - Changes border and card color from gray to tier color
  The onMidpoint callback in CardAnimator.PlayFlip handles this transition.
  On deal (new round), CardView.ShowFaceDown() reverses this:
  - Shows logo
  - Hides header, itemBg, itemSprite, itemName
  - Resets border and card color to gray
- Purple/Gold: HOLD at zero width for pauseDuration before expanding

**Phase 2 — Expand:**
- localScale.x: 0 → 1 (EaseOutQuad)
- Duration: flipDuration / 2
- Visually: card widens as if completing rotation toward viewer

**Tier-specific behavior:**

GREEN (Evasion, Splash Damage, Shield):
- Duration: 0.3s total
- No pause at midpoint
- Quick, snappy feel

BLUE (Chain, Pierce, Rapid Fire):
- Duration: 0.5s total
- No pause at midpoint
- Moderate pace

PURPLE (Vampire, Titan):
- Duration: 0.8s total
- Holds at zero width for 0.15s at midpoint → suspense
- Sequence: compress → pause → expand

GOLD (Headshot, Sniper):
- Duration: 1.2s total
- Screen DIMS SLIGHTLY BEFORE the flip begins
- Holds at zero width for 0.3s at midpoint → longer suspense
- When card expands to reveal: particle BURST or GLOW effect around card
- Sequence: dim screen → compress → long pause → expand + particle → undim

### 3. DISCARD — Card exits screen
- Triggered when player presses "Next Round" button
- Card slides OFF THE TOP of the screen
- Movement ACCELERATES (EaseInCubic) — starts slowly, speeds up
- Duration: discardDuration (default 0.3s)

## Color Values for Card Face
| Tier | Items | Hex | Use for |
|------|-------|-----|---------|
| Gold | Headshot, Sniper | #fcc325 | Card border + background when face-up |
| Purple | Vampire, Titan | #ba26ff | Card border + background when face-up |
| Blue | Chain, Pierce, RapidFire | #008aff | Card border + background when face-up |
| Green | Evasion, SplashDmg, Shield | #5cb85c | Card border + background when face-up |
| Card Back | All (face-down) | #69748c | Border gray when face-down |
