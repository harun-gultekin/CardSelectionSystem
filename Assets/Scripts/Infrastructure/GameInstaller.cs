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
        [SerializeField] private CardView cardView;
        [SerializeField] private GameplayController gameplayController;
        [SerializeField] private DebugPanel debugPanel;
        private void Start()
        {
            var blockCalculator = new BlockCalculator();
            var distributor = new CardDistributor(blockCalculator);
            var validator = new DistributionValidator(blockCalculator);
            var saveService = new JsonSaveService();
            var itemPool = ItemPoolFactory.CreateDefaultPool();
            var gameManager = new GameManager(distributor, validator, saveService, itemPool);

            debugPanel.Initialize(gameManager, validator, itemPool);
            gameplayController.Initialize(gameManager);
        }
    }
}
