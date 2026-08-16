using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AutoScrollToSelected : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    public float padding = 20f;   // Adjust this (20-40 usually feels good)
    private GameObject lastSelected;

    private void Update()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null || selected == lastSelected)
            return;

        lastSelected = selected;

        RectTransform selectedRect = selected.GetComponent<RectTransform>();

        if (selectedRect == null)
            return;

        // Already fully visible? Do nothing.
        if (IsVisible(selectedRect))
            return;

        ScrollTo(selectedRect);
    }

    private bool IsVisible(RectTransform target)
    {
        RectTransform viewport = scrollRect.viewport;

        Vector3[] viewportCorners = new Vector3[4];
        Vector3[] targetCorners = new Vector3[4];

        viewport.GetWorldCorners(viewportCorners);
        target.GetWorldCorners(targetCorners);

        float viewportTop = viewportCorners[1].y;
        float viewportBottom = viewportCorners[0].y;

        float targetTop = targetCorners[1].y;
        float targetBottom = targetCorners[0].y;

        return targetTop <= viewportTop && targetBottom >= viewportBottom;
    }

    private void ScrollTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        RectTransform viewport = scrollRect.viewport;

        Vector3[] viewportCorners = new Vector3[4];
        Vector3[] targetCorners = new Vector3[4];

        viewport.GetWorldCorners(viewportCorners);
        target.GetWorldCorners(targetCorners);

        float viewportTop = viewportCorners[1].y;
        float viewportBottom = viewportCorners[0].y;

        float targetTop = targetCorners[1].y;
        float targetBottom = targetCorners[0].y;

        

        float offset = 0f;

        if (targetTop > viewportTop)
        {
            // Selected item is above the viewport
            offset = targetTop - viewportTop + padding;
        }
        else if (targetBottom < viewportBottom)
        {
            // Selected item is below the viewport
            offset = targetBottom - viewportBottom - padding;
        }

        float normalizedOffset =
            offset / (scrollRect.content.rect.height - viewport.rect.height);

        scrollRect.verticalNormalizedPosition += normalizedOffset;
        scrollRect.verticalNormalizedPosition =
            Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
    }
}