# Persistence System

## Hard Rules (from case doc)
- PlayerPrefs FORBIDDEN
- Save as JSON file in Application.persistentDataPath
- Must survive app quit and relaunch
- Resume EXACTLY where left off

## What Must Persist
1. The complete pre-generated 50-card sequence (all item names in order)
2. The current round number within the cycle (1-50)

## Save Timing — PRECISE
Save on each round completion: "after the player has tapped the card
and the reveal animation is done."
NOT after discard. NOT before flip. After flip/reveal completes.

Flow: player taps → flip animation plays → flip done → SAVE HERE → 
show "Next Round" button → player presses → discard animation → next round

## JSON Structure
```json
{
  "cycleSequence": ["Shield", "Evasion", "SplashDamage", "Pierce", ...],
  "currentRound": 12
}
```
- cycleSequence: array of 50 item name strings
- currentRound: 1-indexed (1 = first round, 50 = last round)

## Load Flow
App start → TryLoad():
  → File exists AND valid JSON AND sequence length == 50
    → Resume: use saved sequence, start at saved round
  → File missing OR corrupt OR invalid
    → Generate new cycle, set round = 1, save immediately

## Cycle End Flow
Round 50 completed → save (round=50) → generate new cycle → 
set round = 1 → save (new sequence, round=1) → continue to round 1

## Error Handling
- JSON parse exception → log warning, generate new cycle
- File read/write exception → log warning, generate new cycle
- Sequence length != 50 → treat as corrupt, generate new cycle

## ISaveService Interface
```csharp
public interface ISaveService {
    void Save(CycleState state);
    CycleState Load();  // returns null if no valid save exists
}
```

## File Path
Application.persistentDataPath + "/save.json"
Constructor should accept custom path for unit testing.
