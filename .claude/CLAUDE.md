# Card Selection System — Unity Project

## Project Summary
Card-based reward system for a 2D mobile game. 50-card cycles, 10 items,
constraint-based distribution algorithm, color-tier animations with distinct
feel per rarity. Single scene, no menu, no audio.

## Tech Stack
- Unity 2022.3 LTS, 2D
- C# — constructor injection (DI), no singletons, no FindObjectOfType
- DOTween (or manual tween) — Animator/Animation component FORBIDDEN
- JSON persistence — PlayerPrefs FORBIDDEN
- Portrait mode (3:4 to 9:21 aspect ratios)

## Folder Structure
- Scripts/Core/ → pure game logic, minimal Unity dependency
- Scripts/Core/Distribution/ → algorithm, block calculation, validation
- Scripts/Core/Models/ → data classes, enums, configs
- Scripts/Core/Persistence/ → JSON save/load
- Scripts/Presentation/ → UI, animation, visuals (MonoBehaviours)
- Scripts/Infrastructure/ → DI wiring (GameInstaller)
- Config/ → ScriptableObject assets (ItemDatabase, individual ItemData assets)
- Tests/EditMode/ → unit tests (NUnit, no scene required)

## Dependency Rule
Core does NOT know Presentation. Presentation depends on Core interfaces.
Infrastructure wires them together. All game logic classes receive
dependencies through constructors — no FindObjectOfType, no static
singletons, no scene-search patterns.

## Memory Files
- @memories/architecture.md → DI, separation of concerns, class responsibilities
- @memories/distribution.md → algorithm, block calculation, validation, impossible detection
- @memories/animation.md → deal/flip/discard specs, easing curves, tier behaviors
- @memories/persistence.md → JSON save/load, timing, error handling
- @memories/item-data.md → item pool, colors, sprites, abbreviations
- @memories/game-flow.md → round sequence, debug panel, screen adaptation

## Evaluation Priorities (company's stated order)
1. Distribution algorithm — correctness, overlap handling, impossible detection, unit tests
2. Code organization — separation of concerns, readable naming
3. Card flip animation — smooth, tier-distinct, dramatic gold reveal
4. Persistence — resume exactly where left off
5. Animation variety — deal, flip, discard feel different

## Critical Rules
- Animation parameters MUST NOT be hardcoded → [SerializeField] with defaults
- Distribution logic MUST NOT live in UI/display scripts
- File I/O MUST NOT live in UI/display scripts
- Block calculations MUST NOT live in UI/display scripts
- Unit tests for distribution are a significant plus
- Algorithm must detect impossible configurations
- At least 3 visually distinct easing curves across animations
- Item configuration is data-driven via ScriptableObjects — adding/removing items requires zero code changes
- CardAnimator is created in GameInstaller and injected into GameplayController
