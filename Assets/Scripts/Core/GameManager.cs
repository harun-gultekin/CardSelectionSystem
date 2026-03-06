using System;
using System.Collections.Generic;
using System.Linq;
using CardSelectionSystem.Core.Distribution;
using CardSelectionSystem.Core.Models;
using CardSelectionSystem.Core.Persistence;

namespace CardSelectionSystem.Core
{
    public class GameManager
    {
        private readonly IDistributor distributor;
        private readonly IDistributionValidator validator;
        private readonly ISaveService saveService;
        private readonly List<ItemConfig> itemPool;

        public string[] CurrentSequence { get; private set; }
        public int CurrentRound { get; private set; }
        public string CurrentItemName => CurrentSequence[CurrentRound - 1];
        public bool IsCycleComplete => CurrentRound > 50;

        public event Action<int> OnRoundChanged;
        public event Action OnCycleCompleted;

        public GameManager(
            IDistributor distributor,
            IDistributionValidator validator,
            ISaveService saveService,
            List<ItemConfig> itemPool)
        {
            this.distributor = distributor;
            this.validator = validator;
            this.saveService = saveService;
            this.itemPool = itemPool;
        }

        public void Initialize()
        {
            var savedState = saveService.Load();

            if (savedState != null && savedState.CycleSequence != null && savedState.CycleSequence.Length == 50)
            {
                CurrentSequence = savedState.CycleSequence;
                CurrentRound = savedState.CurrentRound;
            }
            else
            {
                StartNewCycle();
            }
        }

        public ItemConfig GetCurrentItem()
        {
            var itemName = CurrentItemName;
            return itemPool.FirstOrDefault(item => item.Name == itemName);
        }

        public void CompleteRound()
        {
            CurrentRound++;

            if (CurrentRound > 50)
            {
                OnCycleCompleted?.Invoke();
                StartNewCycle();
            }
            else
            {
                saveService.Save(new CycleState(CurrentSequence, CurrentRound));
            }

            OnRoundChanged?.Invoke(CurrentRound);
        }

        public void StartNewCycle()
        {
            CurrentSequence = distributor.Generate(itemPool);
            CurrentRound = 1;
            saveService.Save(new CycleState(CurrentSequence, CurrentRound));
        }
    }
}
