using UnityEngine;
using TMPro;

namespace CardSelectionSystem.Presentation
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer cardBackgroundRenderer;
        [SerializeField] private SpriteRenderer itemSpriteRenderer;
        [SerializeField] private TextMeshPro itemNameText;
        [SerializeField] private Sprite cardBackSprite;
        [SerializeField] private Color cardBackBorderColor = new Color(0.412f, 0.455f, 0.549f, 1f); // #69748c

        private void Awake()
        {
            ShowFaceDown();
        }

        public void ShowFaceDown()
        {
            if (cardBackgroundRenderer != null)
            {
                cardBackgroundRenderer.sprite = cardBackSprite;
                cardBackgroundRenderer.color = cardBackBorderColor;
            }

            if (itemSpriteRenderer != null)
            {
                itemSpriteRenderer.enabled = false;
            }

            if (itemNameText != null)
            {
                itemNameText.enabled = false;
            }
        }

        public void ShowFaceUp(string itemName, Sprite itemSprite, Color tierColor)
        {
            if (cardBackgroundRenderer != null)
            {
                cardBackgroundRenderer.color = tierColor;
            }

            if (itemSpriteRenderer != null)
            {
                itemSpriteRenderer.sprite = itemSprite;
                itemSpriteRenderer.enabled = true;
            }

            if (itemNameText != null)
            {
                itemNameText.text = itemName;
                itemNameText.enabled = true;
            }
        }

        public void SetVisible(bool visible)
        {
            if (cardBackgroundRenderer != null)
                cardBackgroundRenderer.enabled = visible;

            if (itemSpriteRenderer != null)
                itemSpriteRenderer.enabled = visible;

            if (itemNameText != null)
                itemNameText.enabled = visible;
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}
