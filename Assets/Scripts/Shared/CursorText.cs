using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public TextMeshProUGUI text;

    public Color defaultColour = new Color(0f, 0f, 0f, 255f); // black
    public Color highlightColour = new Color(153f, 255f, 255f, 255f);
    public Color pressedColour = new Color(120f, 200f, 200f, 255f);

    private Vector3 originalScale;
    public float enlargeScale;

    public void Start()
    {
        originalScale = text.rectTransform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = highlightColour;
        text.rectTransform.localScale = originalScale * enlargeScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = defaultColour;
        text.rectTransform.localScale = originalScale / enlargeScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        text.color = pressedColour;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        text.color = defaultColour;
    }
}