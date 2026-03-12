using System.Collections.Generic;
using UnityEngine;
using CardSelectionSystem.Core;
using CardSelectionSystem.Presentation.StateMachine;

namespace CardSelectionSystem.Presentation
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] private CardView cardView;
        [SerializeField] private ScreenDimEffect screenDimEffect;
        [SerializeField] private GoldRevealEffect goldRevealEffect;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private UnityEngine.UI.Button nextRoundButtonComponent;

        private IGameState currentState;
        private GameContext context;

        public void Initialize(GameManager gameManager, CardAnimator cardAnimator,
            Dictionary<string, Sprite> itemSprites)
        {
            Vector3 cardHomePosition = cardView.GetTransform().position;

            context = new GameContext(
                cardView, cardAnimator, gameManager,
                screenDimEffect, goldRevealEffect, mainCamera,
                nextRoundButtonComponent, itemSprites, cardHomePosition
            );

            context.RequestTransition = TransitionTo;
            nextRoundButtonComponent.onClick.AddListener(() => currentState?.OnNextRoundPressed());

            TransitionTo(new DealingState(context));
        }

        private void TransitionTo(IGameState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        private void Update()
        {
            currentState?.Update();
        }
    }
}
