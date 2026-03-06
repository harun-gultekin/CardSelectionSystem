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
            roundText.text = $"Round {gameManager.CurrentRound} / 50";

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

            var result = validator.Validate(sequence, itemPool, 50);

            if (result.IsValid)
                validityText.text = "All 50/50 in block \u2705";
            else
                validityText.text = $"{result.ValidCount}/50 in block \u274C";
        }

        private string GetAbbreviation(string itemName)
        {
            var config = itemPool.FirstOrDefault(item => item.Name == itemName);
            return config != null ? config.Abbreviation : "??";
        }
    }
}
