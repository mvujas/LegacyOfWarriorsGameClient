using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardController))]
public class CardDragger : MouseInteractableCard
{
    private CardController cardController = null;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    public void SavePosition()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localEulerAngles;
    }

    protected override void OnPointerDownCallback(PointerEventData eventData)
    {
        var place = cardController.cardPlace;
    }

    protected override void OnPointerUpCallback(PointerEventData eventData)
    {
        var place = cardController.cardPlace;
        if (place == ClientSideCardPlace.HAND)
        {
            transform.localPosition = initialPosition;
            transform.localEulerAngles = initialRotation;
        }
    }

    protected override void OnPointerDragCallback()
    {
        var place = cardController.cardPlace;
        if (place == ClientSideCardPlace.HAND)
        {
            transform.position = Input.mousePosition;
            transform.eulerAngles = Vector3.zero;
        }
    }

    private void Awake()
    {
        cardController = GetComponent<CardController>();
    }

    private void Start()
    {
        SavePosition();
    }
}
