using System;
using UnityEngine;
using DG.Tweening;
using CardSelectionSystem.Core.Models;

namespace CardSelectionSystem.Presentation
{
    public class CardAnimator
    {
        private readonly AnimationConfig config;

        public CardAnimator(AnimationConfig config)
        {
            this.config = config;
        }

        public Sequence PlayDeal(Transform cardTransform, Vector3 targetPosition, float screenBottomY)
        {
            cardTransform.DOKill();

            cardTransform.position = new Vector3(targetPosition.x, screenBottomY, 0f);

            var sequence = DOTween.Sequence();
            sequence.Append(
                cardTransform.DOMove(targetPosition, config.DealDuration)
                    .SetEase(Ease.OutCubic)
            );
            return sequence;
        }

        public Sequence PlayFlip(Transform cardTransform, CardTier tier, Action onMidpoint, Action onGoldStart = null)
        {
            cardTransform.DOKill();

            float flipDuration = config.GetFlipDuration(tier);
            float halfDuration = flipDuration / 2f;
            float midpointPause = config.GetMidpointPause(tier);

            var sequence = DOTween.Sequence();

            if (tier == CardTier.Gold && onGoldStart != null)
            {
                sequence.AppendCallback(() => onGoldStart.Invoke());
            }

            // Phase 1: Compress scale.x from 1 to 0
            sequence.Append(
                cardTransform.DOScaleX(0f, halfDuration)
                    .SetEase(Ease.InQuad)
            );

            // Midpoint: swap sprites via callback
            sequence.AppendCallback(() => onMidpoint?.Invoke());

            // Midpoint pause for Purple/Gold
            if (midpointPause > 0f)
            {
                sequence.AppendInterval(midpointPause);
            }

            // Phase 2: Expand scale.x from 0 to 1
            sequence.Append(
                cardTransform.DOScaleX(1f, halfDuration)
                    .SetEase(Ease.OutQuad)
            );

            return sequence;
        }

        public Sequence PlayDiscard(Transform cardTransform, float screenTopY)
        {
            cardTransform.DOKill();

            Vector3 target = new Vector3(cardTransform.position.x, screenTopY, 0f);

            var sequence = DOTween.Sequence();
            sequence.Append(
                cardTransform.DOMove(target, config.DiscardDuration)
                    .SetEase(Ease.InCubic)
            );
            return sequence;
        }
    }
}
