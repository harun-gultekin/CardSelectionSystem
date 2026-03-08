using UnityEngine;
using CardSelectionSystem.Core;
using CardSelectionSystem.Core.Distribution;
using CardSelectionSystem.Core.Models;
using CardSelectionSystem.Core.Persistence;
using CardSelectionSystem.Presentation;

namespace CardSelectionSystem.Infrastructure
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private ItemDatabase itemDatabase;
        [SerializeField] private AnimationConfig animationConfig;
        [SerializeField] private CardView cardView;
        [SerializeField] private GameplayController gameplayController;
        [SerializeField] private DebugPanel debugPanel;

        private void Start()
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;

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
}
