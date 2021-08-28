using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Window : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private RectTransform DragRectTransform;
    private Canvas Canvas;

    void Start()
    {
        DragRectTransform = GetComponent<RectTransform>();
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragRectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }
}
