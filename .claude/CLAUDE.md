# Card Selection System — Unity Project

## Project Summary
Card-based reward system for a 2D mobile game. 50-card cycles, 10 items,
constraint-based distribution algorithm, color-tier animations.

## Tech Stack
- Unity 2022.3 LTS, 2D
- C# — constructor injection (DI), no singletons, no FindObjectOfType
- DOTween (or manual tween) — Animator/Animation FORBIDDEN
- JSON persistence — PlayerPrefs FORBIDDEN
- Portrait mode (3:4 to 9:21 aspect ratios)

## Folder Structure
- Scripts/Core/ → pure game logic, minimal Unity dependency
- Scripts/Presentation/ → UI, animation, visuals
- Scripts/Infrastructure/ → DI wiring
- Scripts/Tests/ → unit tests

## Dependency Rule
Core does NOT know Presentation. Presentation depends on Core interfaces.
Infrastructure wires them together.

## Memory Files
- @memories/architecture.md → architecture questions
- @memories/distribution.md → algorithm work
- @memories/animation.md → animation system
- @memories/persistence.md → save/load system
- @memories/item-data.md → item data and color codes

## Critical Rules
- Animation parameters MUST NOT be hardcoded → [SerializeField] or ScriptableObject
- Distribution logic MUST NOT be in UI code
- Every class receives dependencies through constructor
- Unit tests required for distribution + persistence
