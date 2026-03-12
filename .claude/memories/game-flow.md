# Game Flow & UI

## Round Sequence (exact order from case doc)
Each round follows this sequence:
1. DEAL — face-down card appears on screen (slides up from below)
2. WAIT — player taps the card
3. REVEAL — card flips to show color and item (tier-dependent animation)
4. SAVE — state saved to JSON (after reveal animation completes)
5. CONFIRM — "Next Round" button appears on screen
6. DISCARD — player presses button, card animates off-screen (slides up)
7. REPEAT from step 1

After round 50: cycle ends → new cycle auto-generated → round 1 begins.

## Input
- Player TAPS the card to trigger flip (step 2→3)
- Player PRESSES "Next Round" button to trigger discard (step 5→6)
- No other input needed (no menu, no navigation)

## "Next Round" Button
- Appears AFTER flip/reveal animation completes
- Disappears when discard begins
- Triggers discard animation on press
- After discard animation completes → deal next card

## Cycle Boundary
- Round 50 is the last round in a cycle
- After round 50 reveal + save → auto-generate new 50-card sequence
- New cycle starts at round 1 immediately
- No special UI or transition screen needed (unless you want to add polish)

## Debug Panel (visible during gameplay)
Must show three things:

### 1. Round Counter
Format: "Round 12 / 50"

### 2. Full Sequence Display
Show all 50 cards as abbreviations with current card highlighted.
Example: `Ev Sh Sp Pi Ti Rf Ch Ev Sh Sp ...`
With position 12 highlighted (different color, bold, underline, or background).
Use abbreviations from item-data.md: Hs, Sn, Va, Ti, Ch, Pi, Rf, Ev, Sp, Sh

### 3. Block Validity Indicator
Show whether ALL item-instances are within their designated blocks.
Examples:
- "All 50/50 in block ✅" (all valid)
- "48/50 in block ❌" (some invalid — should never happen with correct algorithm)

## Screen Adaptation
- Portrait mode ONLY
- Must display properly on BOTH:
  - Wide devices: 3:4 aspect ratio (e.g., iPad)
  - Narrow devices: 9:21 aspect ratio (e.g., modern phones)
- Card should be reasonably sized on both
- Consider using Camera.orthographicSize or Canvas scaling
- Debug panel should not overlap with card area

## State Machine
Each round phase is a separate state class implementing IGameState.
Transitions are explicit: each state creates the next state and requests transition.
New features (skip, undo, etc.) only require adding methods to IGameState and
implementing per-state — existing state code doesn't need modification (Open/Closed Principle).

## What NOT to Build
- No main menu or navigation flow
- No sound effects or music
