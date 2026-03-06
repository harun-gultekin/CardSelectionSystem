using System;
using UnityEngine;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Presentation
{
    [Serializable]
    public class AnimationConfig
    {
        [SerializeField] private float dealDuration = 0.4f;
        [SerializeField] private float greenFlipDuration = 0.3f;
        [SerializeField] private float blueFlipDuration = 0.5f;
        [SerializeField] private float purpleFlipDuration = 0.8f;
        [SerializeField] private float purpleMidpointPause = 0.15f;
        [SerializeField] private float goldFlipDuration = 1.2f;
        [SerializeField] private float goldMidpointPause = 0.3f;
        [SerializeField] private float discardDuration = 0.3f;

        public float DealDuration => dealDuration;
        public float GreenFlipDuration => greenFlipDuration;
        public float BlueFlipDuration => blueFlipDuration;
        public float PurpleFlipDuration => purpleFlipDuration;
        public float PurpleMidpointPause => purpleMidpointPause;
        public float GoldFlipDuration => goldFlipDuration;
        public float GoldMidpointPause => goldMidpointPause;
        public float DiscardDuration => discardDuration;

        public float GetFlipDuration(CardTier tier)
        {
            switch (tier)
            {
                case CardTier.Green:  return greenFlipDuration;
                case CardTier.Blue:   return blueFlipDuration;
                case CardTier.Purple: return purpleFlipDuration;
                case CardTier.Gold:   return goldFlipDuration;
                default:              return greenFlipDuration;
            }
        }

        public float GetMidpointPause(CardTier tier)
        {
            switch (tier)
            {
                case CardTier.Purple: return purpleMidpointPause;
                case CardTier.Gold:   return goldMidpointPause;
                default:              return 0f;
            }
        }
    }
}
