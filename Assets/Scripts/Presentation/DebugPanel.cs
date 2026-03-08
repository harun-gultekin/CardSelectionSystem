using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using CardSelectionSystem.Core;
using CardSelectionSystem.Core.Distribution;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Presentation
{
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roundText;
        [SerializeField] private TextMeshProUGUI sequenceText;
        [SerializeField] private TextMeshProUGUI validityText;

        private GameManager gameManager;
        private IDistributionValidator validator;
        private List<ItemConfig> itemPool;

        public void Initialize(GameManager gameManager, IDistributionValidator validator, List<ItemConfig> itemPool)
        {
            this.gameManager = gameManager;
            this.validator = validator;
            this.itemPool = itemPool;

            gameManager.OnRoundChanged += _ => UpdateDisplay();
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            int total = gameManager.TotalPositions;
            roundText.text = $"Round {gameManager.CurrentRound} / {total}";

            var sequence = gameManager.CurrentSequence;
            var sb = new StringBuilder();
            int currentIndex = gameManager.CurrentRound - 1;

            for (int i = 0; i < sequence.Length; i++)
            {
                if (i > 0) sb.Append(' ');

                var abbrev = GetAbbreviation(sequence[i]);

                if (i == currentIndex)
                    sb.Append($"<color=yellow>{abbrev}</color>");
                else
                    sb.Append(abbrev);
            }

            sequenceText.text = sb.ToString();

            var result = validator.Validate(sequence, itemPool, total);

            if (result.IsValid)
                validityText.text = $"<color=green>All {total}/{total} in block [OK]</color>";
            else
                validityText.text = $"<color=red>{result.ValidCount}/{total} in block [FAIL]</color>";
        }

        private string GetAbbreviation(string itemName)
        {
            var config = itemPool.FirstOrDefault(item => item.Name == itemName);
            return config != null ? config.Abbreviation : "??";
        }
    }
}
