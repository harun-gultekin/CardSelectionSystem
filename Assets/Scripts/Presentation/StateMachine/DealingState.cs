using UnityEngine;
using DG.Tweening;

namespace CardSelectionSystem.Presentation.StateMachine
{
    public class DealingState : IGameState
    {
        private readonly GameContext context;

        public DealingState(GameContext context)
        {
            this.context = context;
        }

        public void Enter()
        {
            context.NextRoundButton.gameObject.SetActive(false);
            context.CardView.SetVisible(true);
            context.CardView.ShowFaceDown();

            Transform cardTransform = context.CardView.GetTransform();
            cardTransform.localScale = Vector3.one;

            context.CardAnimator.PlayDeal(
                cardTransform, context.CardHomePosition, context.GetScreenBottomY()
            ).OnComplete(() =>
            {
                context.RequestTransition(new WaitingForTapState(context));
            });
        }

        public void Exit() { }
        public void Update() { }
        public void OnCardTapped() { }
        public void OnNextRoundPressed() { }
    }
}
