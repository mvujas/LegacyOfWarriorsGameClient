using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CardController))]
public class CardBiggerRepresentation : MonoBehaviourWithAddOns
{
    private CardController m_cardController;
    private Vector3 initialScale;

    private void Awake()
    {
        m_cardController = GetComponent<CardController>();
        initialScale = transform.localScale;
    }

    private void Start()
    {
        HideByScaling();
    }

    private void ShowByScaling()
    {
        transform.localScale = initialScale;
    }

    public void ShowBigger(CardController cardController)
    {
        m_cardController.ReplicateStats(cardController);
        ShowByScaling();
    }

    public void HideByScaling()
    {
        transform.localScale = Vector3.zero;
    }
}
