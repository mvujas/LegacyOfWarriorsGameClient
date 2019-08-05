using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class CardDragger : MouseInteractableCard
{
    private Vector3 delta;
    private Vector3 initialPosition;
    private Vector3 initialRotation;

    public void SavePosition()
    {
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }

    protected override void OnPointerDownCallback(PointerEventData eventData)
    {
        delta = new Vector3(eventData.delta.x, eventData.delta.y);
    }

    protected override void OnPointerUpCallback(PointerEventData eventData)
    {
        transform.position = initialPosition;
        transform.eulerAngles = initialRotation;
    }

    protected override void OnPointerDragCallback()
    {
        transform.position = Input.mousePosition + delta;
        transform.eulerAngles = Vector3.zero;
    }

    private void Start()
    {
        SavePosition();
    }
}
