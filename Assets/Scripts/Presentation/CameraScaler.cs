using UnityEngine;

namespace CardSelectionSystem.Presentation
{
    public class CameraScaler : MonoBehaviour
    {
        [SerializeField] private float cardWorldWidth = 4.82f;
        [SerializeField] private float cardWorldHeight = 6.52f;
        [SerializeField] private float padding = 0.15f;
        [SerializeField] private float uiSpaceMultiplier = 1.3f;
        [SerializeField] private Camera targetCamera;

        private void Start()
        {
            AdjustCamera();
        }

        private void AdjustCamera()
        {
            float screenAspect = (float)Screen.width / Screen.height;
            float cardAspect = cardWorldWidth / cardWorldHeight;

            float orthoSize;
            if (screenAspect < cardAspect)
            {
                // Narrow screen (e.g. 9:21 phone) — fit by width
                orthoSize = (cardWorldWidth / screenAspect) / 2f;
            }
            else
            {
                // Wide screen (e.g. 3:4 tablet) — fit by height
                orthoSize = cardWorldHeight / 2f;
            }

            orthoSize *= (1f + padding) * uiSpaceMultiplier;
            targetCamera.orthographicSize = orthoSize;
        }
    }
}
