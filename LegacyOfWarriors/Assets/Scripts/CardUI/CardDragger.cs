using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class CardDragger : MouseInteractableCard
{
    private Vector3 initialPosition;
    private Vector3 initialRotation;

    public void SavePosition()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localEulerAngles;
    }

    protected override void OnPointerDownCallback(PointerEventData eventData)
    {
    }

    protected override void OnPointerUpCallback(PointerEventData eventData)
    {
        transform.localPosition = initialPosition;
        transform.localEulerAngles = initialRotation;
    }

    protected override void OnPointerDragCallback()
    {
        transform.position = Input.mousePosition;
        transform.eulerAngles = Vector3.zero;
    }

    private void Start()
    {
        SavePosition();
    }
}
