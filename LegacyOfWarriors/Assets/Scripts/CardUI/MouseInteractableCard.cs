using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public abstract class MouseInteractableCard : MonoBehaviourWithAddOns, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool m_isOver = false;
    private bool m_isDragging = false;

    protected virtual void Update()
    {
        if (m_isOver)
        {
            OnPointerOverCallback();
        }
        if (m_isDragging)
        {
            OnPointerDragCallback();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_isOver = true;
        OnPointerEnterCallback(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_isOver = false;
        OnPointerExitCallback(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_isDragging = true;
        OnPointerDownCallback(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_isDragging = false;
        OnPointerUpCallback(eventData);
    }

    protected virtual void OnPointerOverCallback() { }
    protected virtual void OnPointerDragCallback() { }
    protected virtual void OnPointerEnterCallback(PointerEventData eventData) { }
    protected virtual void OnPointerExitCallback(PointerEventData eventData) { }
    protected virtual void OnPointerDownCallback(PointerEventData eventData) { }
    protected virtual void OnPointerUpCallback(PointerEventData eventData) { }

}
