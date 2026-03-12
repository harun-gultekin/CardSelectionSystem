# Card Selection System

## Overview

Card-based reward system for a 2D mobile game built with Unity 2022.3 LTS.
Each round, a face-down card appears. The player taps to flip and claim
the item inside. After completing all rounds in a cycle, a new cycle
begins automatically.

## How to Run

- Open the project in Unity 2022.3 LTS
- Open the scene: `Assets/Scenes/GameScene.unity`
- Press Play
- Tap the card to flip, press "Next Round" to continue

## Architecture

### Layer Separation

The project follows strict separation of concerns with three layers:

**Core** (pure C#, no Unity dependency):
- Distribution algorithm (BlockCalculator, CardDistributor, DistributionValidator)
- Game state management (GameManager)
- Persistence (JsonSaveService)
- Data models (ItemConfig, CycleState, BlockRange, CardTier)

**Presentation** (Unity MonoBehaviours):
- Card visuals (CardView — multi-layered SpriteRenderers)
- Code-driven animations (CardAnimator — DOTween, 4 distinct easing curves)
- Game loop orchestration (GameplayController — Finite State Machine with
  5 state classes: DealingState, WaitingForTapState, FlippingState,
  WaitingForConfirmState, DiscardingState)
- Debug display (DebugPanel — round counter, sequence, block validity)
- Screen adaptation (CameraScaler — dynamic orthographic sizing)
- Gold effects (ScreenDimEffect, GoldRevealEffect — ParticleSystem)

**Infrastructure** (wiring):
- GameInstaller — single entry point, creates all services and injects dependencies

Core does not know about Presentation. Presentation depends on Core interfaces.
Infrastructure wires them together.

### Dependency Injection

Manual constructor injection without a DI framework. All game logic classes
receive dependencies through constructors. GameInstaller creates and wires
everything in `Start()`. No `FindObjectOfType`, no static singletons, no
scene-search patterns.

### Finite State Machine

The gameplay flow uses a proper FSM pattern where each game phase is a
separate class implementing IGameState (Enter, Exit, Update, OnCardTapped,
OnNextRoundPressed). GameContext holds shared dependencies for all states.
GameplayController acts as a thin driver (~45 lines) that manages transitions.

States: Dealing → WaitingForTap → Flipping → WaitingForConfirm → Discarding → Dealing

This replaces the original enum-based approach where all state logic lived
in a single class with if/else checks. The FSM design follows the
Open/Closed Principle — adding features like skip requires only adding a
method to IGameState and implementing it per-state, with zero changes to
existing state code.

CardView detects its own taps via OnMouseDown and fires an OnTapped event.
States subscribe/unsubscribe in Enter/Exit, maintaining clean separation
of concerns.

### Data-Driven Item Configuration

Items are defined as ScriptableObject assets (ItemData). An ItemDatabase
ScriptableObject holds all items. Adding, removing, or modifying items
requires zero code changes — only Inspector configuration. The total card
count per cycle is dynamically calculated from the item pool.

ItemData provides a `ToItemConfig()` bridge method that converts the
ScriptableObject data to pure C# ItemConfig objects used by the algorithm.

## Distribution Algorithm

The algorithm pre-generates a 50-card sequence ensuring even item spacing.
Each item's instances are distributed across designated block windows
using a constraint satisfaction approach:

1. **Block calculation**: each item divides the cycle into equal windows
2. **Most-constrained-first ordering**: narrowest blocks placed first
3. **Recursive backtracking**: guarantees valid placement or detects impossibility
4. **Randomized candidate selection** within blocks for variety

The algorithm handles overlapping block competition and detects impossible
configurations. Validated by 27 unit tests including stress tests.

## Animations

All animations are code-driven using DOTween (no Animator/Animation components).
Four distinct easing curves are used:

- **Deal**: EaseOutCubic (decelerates into position)
- **Flip compress**: EaseInQuad (accelerates to midpoint)
- **Flip expand**: EaseOutQuad (decelerates from midpoint)
- **Discard**: EaseInCubic (accelerates off-screen)

Color tiers provide distinct flip experiences:

- Green: 0.3s, snappy
- Blue: 0.5s, moderate
- Purple: 0.8s, midpoint suspense pause
- Gold: 1.2s, screen dim before flip, longer pause, particle burst on reveal

All animation parameters are editable in the Unity Inspector via AnimationConfig.

## Persistence

Game state (card sequence + current round) saved as JSON in
`Application.persistentDataPath`. Saves after each round completion.
Handles corrupt files, missing saves, and item pool changes gracefully.

## Testing

27 EditMode unit tests covering:

- Block calculation correctness (11 tests)
- Distribution validity, randomness, impossible configs (9 tests)
- JSON persistence save/load/corrupt/delete (7 tests)

Run via Window > General > Test Runner > EditMode > Run All

## Screen Support

Portrait mode, adapts to all aspect ratios from 3:4 (tablets) to 9:21
(narrow phones) via dynamic camera orthographic size calculation.
Safe area handling ensures UI elements avoid notches, Dynamic Island, 
and home bar indicators on modern devices.

## Technical Decisions

### Why manual DI instead of VContainer?

Single-scene project with limited scope. Manual constructor injection
demonstrates DI understanding without unnecessary framework overhead.
VContainer would be appropriate for a larger project.

### Why ScriptableObjects for item data?

Decouples game data from code. Designers can add/remove items without
programmer involvement. The `ToItemConfig()` bridge keeps the algorithm
layer pure C# and fully testable.

### Why Finite State Machine over enum-based states?

The original enum approach put all state logic in one class with if/else
guards. Adding a feature like skip would require touching every method and
adding branches for each state. With the FSM, each state is isolated —
adding skip means implementing OnSkip() per-state without modifying
existing code. The pattern also enforces Enter/Exit symmetry for clean
setup and teardown (e.g., WaitingForConfirmState shows the button in
Enter, hides it in Exit — no other state manages button visibility).

### Why ItemPoolFactory still exists?

Unit tests need pure C# item definitions without Unity Editor dependency.
ItemPoolFactory provides test data. Runtime uses ItemDatabase ScriptableObject.

## AI-Assisted Development

This project was developed with Claude Code as an AI pair-programming tool.
The `.claude/` directory contains project memory files that provide Claude
with architectural context, algorithm specifications, and design constraints.

### Memory File Structure

- `CLAUDE.md` — project overview, tech stack, critical rules
- `memories/architecture.md` — DI pattern, separation of concerns
- `memories/distribution.md` — algorithm details, block calculation
- `memories/animation.md` — animation specs, easing curves, tier behaviors
- `memories/persistence.md` — save/load system
- `memories/item-data.md` — item pool, card visual structure
- `memories/game-flow.md` — round sequence, debug panel, screen adaptation

This structure enables focused, context-aware prompts — each system has
its own memory file that Claude reads before generating or modifying code.

All generated code was reviewed, tested, and understood before submission.
