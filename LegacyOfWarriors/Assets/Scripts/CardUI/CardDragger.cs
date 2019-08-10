using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CardController))]
public class CardDragger : MouseInteractableCard
{
    private CardController cardController = null;
    private MainGameLogicController mainGameLogicController = null;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    private static CardController hoveringController = null;

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
        if (place == ClientSideCardPlace.HAND && mainGameLogicController.IsPlayersTurn && mainGameLogicController.playersDataController.Mana >= cardController.Cost)
        {
            transform.localPosition = initialPosition;
            transform.localEulerAngles = initialRotation;

            globalReference.GameClient.Send(new Remote.Implementation.PlayCardRequest { CardInGameId = cardController.CardInGameId });
        }

        if(place == ClientSideCardPlace.FIELD && mainGameLogicController.IsPlayersTurn && mainGameLogicController.AcccumulativeTurn >= cardController.LastAttackingTurn)
        {
            globalReference.GameClient.Send(new Remote.Implementation.AttackRequest {
                AttackingUnit = cardController.CardInGameId,
                TargetPlayer = 1 - mainGameLogicController.PlayerIndex,
                TargetUnit = (hoveringController?.CardInGameId)
            });
        }
    }

    protected override void OnPointerDragCallback()
    {
        var place = cardController.cardPlace;
        if (place == ClientSideCardPlace.HAND && mainGameLogicController.IsPlayersTurn && mainGameLogicController.playersDataController.Mana >= cardController.Cost)
        {
            transform.position = Input.mousePosition;
            transform.eulerAngles = Vector3.zero;
        }
    }

    protected override void OnPointerEnterCallback(PointerEventData eventData)
    {
        hoveringController = cardController;
    }

    protected override void OnPointerExitCallback(PointerEventData eventData)
    {
        hoveringController = null;
    }

    private void Awake()
    {
        cardController = GetComponent<CardController>();

        GameObject mainLogic = GameObject.FindWithTag("MainGameLogic");
        if(mainLogic == null)
        {
            throw new ArgumentException("There's no main game logic");
        }

        mainGameLogicController = mainLogic.GetComponent<MainGameLogicController>();

        if(mainGameLogicController == null)
        {
            throw new ArgumentException("Main game logic doesn't have MainGameLogicController component");
        }
    }

    private void Start()
    {
        SavePosition();
    }
}
