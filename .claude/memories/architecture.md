# Architecture

## DI Pattern — Manual Constructor Injection
Single GameInstaller MonoBehaviour creates and wires all services in Start().
No DI framework needed. This demonstrates DI understanding without unnecessary overhead.

```csharp
public class GameInstaller : MonoBehaviour {
    [SerializeField] private ItemDatabase itemDatabase;
    [SerializeField] private AnimationConfig animationConfig;
    [SerializeField] private CardView cardView;
    [SerializeField] private GameplayController gameplayController;
    [SerializeField] private DebugPanel debugPanel;

    void Start() {
        var blockCalculator = new BlockCalculator();
        var distributor = new CardDistributor(blockCalculator);
        var validator = new DistributionValidator(blockCalculator);
        var saveService = new JsonSaveService();
        var itemPool = itemDatabase.ToItemConfigList();
        var spriteDictionary = itemDatabase.BuildSpriteDictionary();
        var gameManager = new GameManager(distributor, validator, saveService, itemPool);
        var cardAnimator = new CardAnimator(animationConfig);

        gameManager.Initialize();
        debugPanel.Initialize(gameManager, validator, itemPool);
        gameplayController.Initialize(gameManager, cardAnimator, spriteDictionary);
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
| Core/Models/ItemDatabase | Item data source (ScriptableObject) | Algorithm, animation |
| Infrastructure/GameInstaller | Creates and wires everything | Business logic details |

## Interfaces
- IDistributor → CardDistributor (method: Generate)
- ISaveService → JsonSaveService (methods: Save, Load)
- IDistributionValidator → DistributionValidator (method: Validate)
- ItemData.ToItemConfig() bridges ScriptableObject → pure C# for algorithm
- ItemDatabase.BuildSpriteDictionary() bridges ScriptableObject → presentation

## State Machine Pattern (Presentation Layer)
GameplayController uses a proper FSM with each state as a separate class.
States implement IGameState interface (Enter, Exit, Update, OnCardTapped, OnNextRoundPressed).
GameContext holds shared dependencies. States request transitions via
context.RequestTransition callback. Adding new features (e.g. skip) means
adding a method to IGameState and implementing it per-state — no if/else chains.
States: DealingState → WaitingForTapState → FlippingState → WaitingForConfirmState → DiscardingState → DealingState

## Explicit Case Constraints (Technical Constraints section)
- NO FindObjectOfType
- NO static singletons
- NO scene-search patterns for connecting logic classes
- UI/display scripts must NOT contain: distribution logic, file I/O, block calculations
- All animation parameters editable via Inspector without code changes
- "Readable naming" is an evaluation criterion
