using DG.Tweening;

namespace CardSelectionSystem.Presentation.StateMachine
{
    public class DiscardingState : IGameState
    {
        private readonly GameContext context;

        public DiscardingState(GameContext context)
        {
            this.context = context;
        }

        public void Enter()
        {
            context.GoldRevealEffect.Stop();

            UnityEngine.Transform cardTransform = context.CardView.GetTransform();

            context.CardAnimator.PlayDiscard(cardTransform, context.GetScreenTopY())
                .OnComplete(() =>
                {
                    context.CardView.SetVisible(false);
                    context.RequestTransition(new DealingState(context));
                });
        }

        public void Exit() { }
        public void Update() { }
        public void OnCardTapped() { }
        public void OnNextRoundPressed() { }
    }
}
