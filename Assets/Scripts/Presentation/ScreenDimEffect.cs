using UnityEngine;
using DG.Tweening;

namespace CardSelectionSystem.Presentation
{
    public class ScreenDimEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer dimRenderer;
        [SerializeField] private float dimAlpha = 0.4f;
        [SerializeField] private float fadeDuration = 0.2f;

        private void Awake()
        {
            if (dimRenderer != null)
            {
                Color color = dimRenderer.color;
                color.a = 0f;
                dimRenderer.color = color;
                dimRenderer.enabled = false;
            }
        }

        public Tween FadeIn()
        {
            dimRenderer.enabled = true;
            return dimRenderer.DOFade(dimAlpha, fadeDuration);
        }

        public Tween FadeOut()
        {
            return dimRenderer.DOFade(0f, fadeDuration)
                .OnComplete(() => dimRenderer.enabled = false);
        }
    }
}
