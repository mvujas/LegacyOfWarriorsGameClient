using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Remote.InGameObjects;

public class HandStringController : MonoBehaviourWithAddOns
{
    public CardController cardController = null;

    private void Awake()
    {
        if(cardController == null)
        {
            throw new ArgumentNullException(nameof(cardController));
        }
    }

    public void SetCardStats(CardInGame cardInGame)
    {
        cardController.ReplicateStats(cardInGame);
    }

    public void SetZRotation(float zRotation)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.z = zRotation;
        transform.eulerAngles = eulerRotation;
    }
}
