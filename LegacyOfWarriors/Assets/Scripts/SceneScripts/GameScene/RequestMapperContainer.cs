using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RequestMapperContainer : MonoBehaviourWithAddOns
{
    public MutablePassiveRequestMapper RequestMapper { get; private set; } = new MutablePassiveRequestMapper();
}
