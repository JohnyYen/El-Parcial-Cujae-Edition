using UnityEngine;
using UnityEngine.UI;

public class CreditsPanel : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollSpeed = 30f;
    [SerializeField] private bool autoScroll = true;

    private void OnEnable()
    {
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void Update()
    {
        if (!autoScroll) return;

        if (scrollRect.verticalNormalizedPosition > 0f)
        {
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.unscaledDeltaTime / scrollRect.content.rect.height;
        }
    }
}
