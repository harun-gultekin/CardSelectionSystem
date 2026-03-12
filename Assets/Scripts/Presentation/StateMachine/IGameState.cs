namespace CardSelectionSystem.Presentation.StateMachine
{
    public interface IGameState
    {
        void Enter();
        void Exit();
        void Update();
        void OnCardTapped();
        void OnNextRoundPressed();
    }
}
