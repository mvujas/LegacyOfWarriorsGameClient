using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardController))]
public class CardHighlighter : MouseInteractableCard
{
    private const string cardRepresentationTagName = "Bigger Card Representation";

    private CardBiggerRepresentation biggerRepresentation = null;
    private CardController cardController = null;

    private bool isHighlighted = false;

    protected override void OnPointerEnterCallback(PointerEventData eventData)
    {
        if(cardController.cardPlace == ClientSideCardPlace.HAND || cardController.cardPlace == ClientSideCardPlace.FIELD)
        {
            biggerRepresentation.ShowBigger(cardController);
            isHighlighted = true;
        }
    }

    protected override void OnPointerExitCallback(PointerEventData eventData)
    {
        if(isHighlighted)
        {
            biggerRepresentation.HideByScaling();
            isHighlighted = false;
        }
    }

    private void Awake()
    {
        var biggerRepresentations = GameObject.FindGameObjectsWithTag(cardRepresentationTagName);
        if(biggerRepresentations.Length == 0)
        {
            throw new ArgumentException("No bigger card representation");
        }
        cardController = GetComponent<CardController>();
        biggerRepresentation = biggerRepresentations[0].GetComponent<CardBiggerRepresentation>();
    }
}
