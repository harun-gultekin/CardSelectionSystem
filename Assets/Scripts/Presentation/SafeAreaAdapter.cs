using UnityEngine;

namespace CardSelectionSystem.Presentation
{
    public class SafeAreaAdapter : MonoBehaviour
    {
        void Awake()
        {
            ApplySafeArea();
        }

        void OnRectTransformDimensionsChange()
        {
            ApplySafeArea();
        }

        void ApplySafeArea()
        {
            var rectTransform = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }
    }
}
