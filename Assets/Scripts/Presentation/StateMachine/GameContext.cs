using System;
using System.Collections.Generic;
using UnityEngine;
using CardSelectionSystem.Core;

namespace CardSelectionSystem.Presentation.StateMachine
{
    public class GameContext
    {
        public CardView CardView { get; }
        public CardAnimator CardAnimator { get; }
        public GameManager GameManager { get; }
        public ScreenDimEffect ScreenDimEffect { get; }
        public GoldRevealEffect GoldRevealEffect { get; }
        public Camera MainCamera { get; }
        public UnityEngine.UI.Button NextRoundButton { get; }
        public Dictionary<string, Sprite> ItemSprites { get; }
        public Vector3 CardHomePosition { get; }
        public Action<IGameState> RequestTransition { get; set; }

        public GameContext(CardView cardView, CardAnimator cardAnimator,
            GameManager gameManager, ScreenDimEffect screenDimEffect,
            GoldRevealEffect goldRevealEffect, Camera mainCamera,
            UnityEngine.UI.Button nextRoundButton,
            Dictionary<string, Sprite> itemSprites, Vector3 cardHomePosition)
        {
            CardView = cardView;
            CardAnimator = cardAnimator;
            GameManager = gameManager;
            ScreenDimEffect = screenDimEffect;
            GoldRevealEffect = goldRevealEffect;
            MainCamera = mainCamera;
            NextRoundButton = nextRoundButton;
            ItemSprites = itemSprites;
            CardHomePosition = cardHomePosition;
        }

        public float GetScreenBottomY()
        {
            float bottomY = MainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).y;
            SpriteRenderer sr = CardView.GetComponentInChildren<SpriteRenderer>();
            float cardHeight = sr != null ? sr.bounds.size.y : 2f;
            return bottomY - cardHeight;
        }

        public float GetScreenTopY()
        {
            float topY = MainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, 0f)).y;
            SpriteRenderer sr = CardView.GetComponentInChildren<SpriteRenderer>();
            float cardHeight = sr != null ? sr.bounds.size.y : 2f;
            return topY + cardHeight;
        }

        public Color ParseHexColor(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
                return color;
            return Color.white;
        }
    }
}
