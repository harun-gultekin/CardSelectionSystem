# Architecture

## DI Pattern — Manual Constructor Injection
Single GameInstaller MonoBehaviour creates and wires all services in Awake().

## Separation of Concerns
| Layer | Responsibility | Does NOT Know |
|-------|---------------|---------------|
| Core/Distribution | Algorithm, block calculation | UI, animation, Unity |
| Core/Persistence | JSON read/write | UI, algorithm |
| Core/GameManager | State management (round, cycle) | UI, animation |
| Presentation/CardView | Sprite display | Algorithm, file I/O |
| Presentation/CardAnimator | Tween/easing | Algorithm, state |
| Infrastructure/GameInstaller | Wiring | Business logic |

## Interfaces
- IDistributor → CardDistributor
- ISaveService → JsonSaveService
- ICardAnimator → CardAnimator
