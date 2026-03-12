namespace CardSelectionSystem.Presentation.StateMachine
{
    public class WaitingForTapState : IGameState
    {
        private readonly GameContext context;

        public WaitingForTapState(GameContext context)
        {
            this.context = context;
        }

        public void Enter()
        {
            context.CardView.OnTapped += HandleCardTapped;
        }

        public void Exit()
        {
            context.CardView.OnTapped -= HandleCardTapped;
        }

        public void Update() { }

        private void HandleCardTapped()
        {
            context.RequestTransition(new FlippingState(context));
        }

        public void OnCardTapped() { }
        public void OnNextRoundPressed() { }
    }
}
