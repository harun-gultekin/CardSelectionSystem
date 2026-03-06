using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CardSelectionSystem.Core;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Presentation
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private CardView cardView;
        [SerializeField] private AnimationConfig animationConfig;
        [SerializeField] private ScreenDimEffect screenDimEffect;
        [SerializeField] private GoldRevealEffect goldRevealEffect;
        [SerializeField] private Camera mainCamera;

        [SerializeField] private Sprite[] itemSpriteArray;
        [SerializeField] private string[] itemSpriteNames;

        private GameManager gameManager;
        private CardAnimator cardAnimator;
        private Dictionary<string, Sprite> itemSprites;
        private Vector3 cardHomePosition;
        private GameState currentState;
        private bool isScreenDimmed;

        private enum GameState
        {
            Dealing,
            WaitingForTap,
            Flipping,
            WaitingForConfirm,
            Discarding
        }

        public void Initialize(GameManager gameManager)
        {
            this.gameManager = gameManager;
            cardAnimator = new CardAnimator(animationConfig);

            BuildSpriteDictionary();

            cardHomePosition = cardView.GetTransform().position;

            DealCard();
        }

        private void BuildSpriteDictionary()
        {
            itemSprites = new Dictionary<string, Sprite>();

            if (itemSpriteArray == null || itemSpriteNames == null)
                return;

            int count = Mathf.Min(itemSpriteArray.Length, itemSpriteNames.Length);
            for (int i = 0; i < count; i++)
            {
                itemSprites[itemSpriteNames[i]] = itemSpriteArray[i];
            }
        }

        private void DealCard()
        {
            currentState = GameState.Dealing;

            cardView.SetVisible(true);
            cardView.ShowFaceDown();

            Transform cardTransform = cardView.GetTransform();
            cardTransform.localScale = Vector3.one;
            Vector3 targetPosition = cardHomePosition;
            float screenBottomY = GetScreenBottomY();

            cardAnimator.PlayDeal(cardTransform, targetPosition, screenBottomY)
                .OnComplete(() => currentState = GameState.WaitingForTap);
        }

        private void OnCardTapped()
        {
            if (currentState != GameState.WaitingForTap)
                return;

            currentState = GameState.Flipping;

            ItemConfig currentItem = gameManager.GetCurrentItem();
            string itemName = currentItem.Name;
            CardTier tier = currentItem.Tier;
            Color tierColor = ParseHexColor(currentItem.ColorHex);

            itemSprites.TryGetValue(itemName, out Sprite itemSprite);

            Transform cardTransform = cardView.GetTransform();

            Tween preFadeIn = null;
            System.Action onExpandStart = null;
            if (tier == CardTier.Gold)
            {
                isScreenDimmed = true;
                preFadeIn = screenDimEffect.FadeIn();
                onExpandStart = () => goldRevealEffect.Play();
            }

            cardAnimator.PlayFlip(
                cardTransform,
                tier,
                () => cardView.ShowFaceUp(itemName, itemSprite, tierColor),
                preFadeIn,
                onExpandStart
            ).OnComplete(() =>
            {
                gameManager.CompleteRound();
                currentState = GameState.WaitingForConfirm;
            });
        }

        public void OnNextRoundPressed()
        {
            if (currentState != GameState.WaitingForConfirm)
                return;

            currentState = GameState.Discarding;

            if (isScreenDimmed)
            {
                isScreenDimmed = false;
                screenDimEffect.FadeOut();
                goldRevealEffect.Stop();
            }

            Transform cardTransform = cardView.GetTransform();
            float screenTopY = GetScreenTopY();

            cardAnimator.PlayDiscard(cardTransform, screenTopY)
                .OnComplete(() =>
                {
                    cardView.SetVisible(false);
                    DealCard();
                });
        }

        private void Update()
        {
            if (currentState != GameState.WaitingForTap)
                return;

            if (!Input.GetMouseButtonDown(0))
                return;

            Vector3 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null && hit.collider.transform == cardView.GetTransform())
            {
                OnCardTapped();
            }
        }

        private float GetScreenBottomY()
        {
            float bottomY = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
            SpriteRenderer sr = cardView.GetComponentInChildren<SpriteRenderer>();
            float cardHeight = sr != null ? sr.bounds.size.y : 2f;
            return bottomY - cardHeight;
        }

        private float GetScreenTopY()
        {
            float topY = mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
            SpriteRenderer sr = cardView.GetComponentInChildren<SpriteRenderer>();
            float cardHeight = sr != null ? sr.bounds.size.y : 2f;
            return topY + cardHeight;
        }

        private Color ParseHexColor(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
                return color;
            return Color.white;
        }
    }
}
