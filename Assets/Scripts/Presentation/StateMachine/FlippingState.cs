using System;
using UnityEngine;
using DG.Tweening;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Presentation.StateMachine
{
    public class FlippingState : IGameState
    {
        private readonly GameContext context;
        private bool isScreenDimmed;

        public FlippingState(GameContext context)
        {
            this.context = context;
        }

        public void Enter()
        {
            ItemConfig currentItem = context.GameManager.GetCurrentItem();
            string itemName = currentItem.Name;
            CardTier tier = currentItem.Tier;
            Color tierColor = context.ParseHexColor(currentItem.ColorHex);

            context.ItemSprites.TryGetValue(itemName, out Sprite itemSprite);

            Transform cardTransform = context.CardView.GetTransform();

            Tween preFadeIn = null;
            Action onExpandStart = null;
            if (tier == CardTier.Gold)
            {
                isScreenDimmed = true;
                preFadeIn = context.ScreenDimEffect.FadeIn();
                onExpandStart = () => context.GoldRevealEffect.Play();
            }

            context.CardAnimator.PlayFlip(
                cardTransform,
                tier,
                () => context.CardView.ShowFaceUp(itemName, itemSprite, tierColor),
                preFadeIn,
                onExpandStart
            ).OnComplete(() =>
            {
                if (isScreenDimmed)
                {
                    isScreenDimmed = false;
                    context.ScreenDimEffect.FadeOut();
                }
                context.GameManager.CompleteRound();
                context.RequestTransition(new WaitingForConfirmState(context));
            });
        }

        public void Exit() { }
        public void Update() { }
        public void OnCardTapped() { }
        public void OnNextRoundPressed() { }
    }
}
