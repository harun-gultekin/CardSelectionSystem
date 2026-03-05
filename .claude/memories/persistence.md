# Persistence System

- JSON at Application.persistentDataPath + "/save.json"
- Save after each round (post-flip)
- Load on start → resume or new cycle
- Corrupt file → new cycle + log warning

## JSON: { "cycleSequence": [...50 strings...], "currentRound": 12 }
