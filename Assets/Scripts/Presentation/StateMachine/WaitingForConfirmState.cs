namespace CardSelectionSystem.Presentation.StateMachine
{
    public class WaitingForConfirmState : IGameState
    {
        private readonly GameContext context;

        public WaitingForConfirmState(GameContext context)
        {
            this.context = context;
        }

        public void Enter()
        {
            context.NextRoundButton.gameObject.SetActive(true);
        }

        public void Exit()
        {
            context.NextRoundButton.gameObject.SetActive(false);
        }

        public void Update() { }
        public void OnCardTapped() { }

        public void OnNextRoundPressed()
        {
            context.RequestTransition(new DiscardingState(context));
        }
    }
}
