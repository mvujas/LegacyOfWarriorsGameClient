using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandStringController : MonoBehaviourWithAddOns
{
    [SerializeField]
    private CardController cardController = null;

    private void Awake()
    {
        if(cardController == null)
        {
            throw new ArgumentNullException(nameof(cardController));
        }
    }

    public void SetCardStats(CardController cardController)
    {
        this.cardController.ReplicateStats(cardController);
    }

    public void SetZRotation(float zRotation)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.z = zRotation;
        transform.eulerAngles = eulerRotation;
    }
}
