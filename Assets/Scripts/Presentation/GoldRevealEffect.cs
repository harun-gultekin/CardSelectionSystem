using UnityEngine;
using DG.Tweening;

namespace CardSelectionSystem.Presentation
{
    public class GoldRevealEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem burstParticle;
        [SerializeField] private SpriteRenderer glowRenderer;
        [SerializeField] private float glowFadeDuration = 0.3f;
        [SerializeField] private float glowMaxAlpha = 0.6f;

        public void Play()
        {
            if (burstParticle != null)
            {
                burstParticle.Play();
            }

            if (glowRenderer != null)
            {
                glowRenderer.enabled = true;
                DOTween.Sequence()
                    .Append(glowRenderer.DOFade(glowMaxAlpha, glowFadeDuration * 0.3f))
                    .Append(glowRenderer.DOFade(0f, glowFadeDuration * 0.7f))
                    .OnComplete(() => glowRenderer.enabled = false);
            }
        }

        public void Stop()
        {
            if (burstParticle != null)
            {
                burstParticle.Stop();
            }

            if (glowRenderer != null)
            {
                glowRenderer.enabled = false;
            }
        }
    }
}
