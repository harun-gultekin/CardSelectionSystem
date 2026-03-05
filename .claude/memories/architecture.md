# Architecture

## DI Pattern — Manual Constructor Injection
Single GameInstaller MonoBehaviour creates and wires all services in Awake().
No DI framework needed. This demonstrates DI understanding without unnecessary overhead.

```csharp
public class GameInstaller : MonoBehaviour {
    [SerializeField] private AnimationConfig animConfig;
    [SerializeField] private CardView cardView;
    [SerializeField] private DebugPanel debugPanel;
    
    void Awake() {
        var blockCalculator = new BlockCalculator();
        var distributor = new CardDistributor(blockCalculator);
        var validator = new DistributionValidator(blockCalculator);
        var saveService = new JsonSaveService();
        var gameManager = new GameManager(distributor, validator, saveService);
        var animator = new CardAnimator(animConfig);
        cardView.Initialize(gameManager, animator);
        debugPanel.Initialize(gameManager, validator);
    }
}
```

## Separation of Concerns
| Layer | Responsibility | Does NOT Know About |
|-------|---------------|---------------------|
| Core/Distribution/BlockCalculator | Block range math | UI, animation, Unity, file I/O |
| Core/Distribution/CardDistributor | Placement algorithm | UI, animation, Unity, file I/O |
| Core/Distribution/DistributionValidator | Sequence validation | UI, animation, Unity, file I/O |
| Core/Persistence/JsonSaveService | JSON read/write | UI, animation, algorithm |
| Core/GameManager | Round/cycle state, orchestration | UI, animation details |
| Presentation/CardView | Sprite display, tap input | Algorithm, file I/O, block math |
| Presentation/CardAnimator | Tween/easing animations | Algorithm, state, file I/O |
| Presentation/DebugPanel | Debug info display | Algorithm internals, file I/O |
| Infrastructure/GameInstaller | Creates and wires everything | Business logic details |

## Interfaces
- IDistributor → CardDistributor (method: Generate)
- ISaveService → JsonSaveService (methods: Save, Load)
- IDistributionValidator → DistributionValidator (method: Validate)

## Explicit Case Constraints (Technical Constraints section)
- NO FindObjectOfType
- NO static singletons
- NO scene-search patterns for connecting logic classes
- UI/display scripts must NOT contain: distribution logic, file I/O, block calculations
- All animation parameters editable via Inspector without code changes
- "Readable naming" is an evaluation criterion
