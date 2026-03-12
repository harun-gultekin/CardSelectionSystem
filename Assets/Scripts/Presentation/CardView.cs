using System;
using UnityEngine;
using TMPro;

namespace CardSelectionSystem.Presentation
{
    public class CardView : MonoBehaviour
    {
        public event Action OnTapped;

        [Header("Card Layers (back to front)")]
        [SerializeField] private SpriteRenderer borderRenderer;
        [SerializeField] private SpriteRenderer cardRenderer;
        [SerializeField] private SpriteRenderer headerRenderer;
        [SerializeField] private SpriteRenderer itemBorderRenderer;
        [SerializeField] private SpriteRenderer itemBgRenderer;
        [SerializeField] private SpriteRenderer itemSpriteRenderer;
        [SerializeField] private SpriteRenderer logoRenderer;
        [SerializeField] private TextMeshPro itemNameText;

        [Header("Face-Down Colors")]
        [SerializeField] private Color cardBackBorderColor = new Color(0.412f, 0.455f, 0.549f, 1f); // #69748c

        private void Awake()
        {
            SetSortingOrders();
            ShowFaceDown();
        }

        private void SetSortingOrders()
        {
            if (cardRenderer != null) cardRenderer.sortingOrder = 0;
            if (borderRenderer != null) borderRenderer.sortingOrder = 1;
            if (headerRenderer != null) headerRenderer.sortingOrder = 1;
            if (itemBgRenderer != null) itemBgRenderer.sortingOrder = 1;
            if (logoRenderer != null) logoRenderer.sortingOrder = 2;
            if (itemNameText != null) itemNameText.sortingOrder = 2;
            if (itemBorderRenderer != null) itemBorderRenderer.sortingOrder = 3;
            if (itemSpriteRenderer != null) itemSpriteRenderer.sortingOrder = 3;
        }

        public void ShowFaceDown()
        {
            if (borderRenderer != null)
            {
                borderRenderer.enabled = true;
                borderRenderer.color = cardBackBorderColor;
            }

            if (cardRenderer != null)
            {
                cardRenderer.enabled = true;
                cardRenderer.color = cardBackBorderColor;
            }

            if (logoRenderer != null)
                logoRenderer.enabled = true;

            if (headerRenderer != null)
                headerRenderer.enabled = false;

            if (itemBorderRenderer != null)
                itemBorderRenderer.enabled = false;

            if (itemBgRenderer != null)
                itemBgRenderer.enabled = false;

            if (itemSpriteRenderer != null)
                itemSpriteRenderer.enabled = false;

            if (itemNameText != null)
                itemNameText.enabled = false;
        }

        public void ShowFaceUp(string itemName, Sprite itemSprite, Color tierColor)
        {
            if (borderRenderer != null)
            {
                borderRenderer.enabled = true;
                borderRenderer.color = tierColor;
            }

            if (cardRenderer != null)
            {
                cardRenderer.enabled = true;
                cardRenderer.color = tierColor;
            }

            if (itemBorderRenderer != null)
            {
                itemBorderRenderer.enabled = true;
                itemBorderRenderer.color = tierColor;
            }

            if (logoRenderer != null)
                logoRenderer.enabled = false;

            if (headerRenderer != null)
            {
                headerRenderer.enabled = true;
                headerRenderer.color = Color.white;
            }

            if (itemBgRenderer != null)
            {
                itemBgRenderer.enabled = true;
                itemBgRenderer.color = Color.white;
            }

            if (itemSpriteRenderer != null)
            {
                itemSpriteRenderer.enabled = true;
                itemSpriteRenderer.sprite = itemSprite;
            }

            if (itemNameText != null)
            {
                itemNameText.enabled = true;
                itemNameText.text = itemName;
            }
        }

        private void OnMouseDown()
        {
            OnTapped?.Invoke();
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}
