using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextColorTransitionable : ColorTransitionable
{
    [SerializeField]
    private Text text = null;

    protected override void Awake()
    {
        if(text == null)
        {
            Debug.Log("Text is not set in TextColorTransitionable");
        }
        base.Awake();
    }

    internal override Color GetCurrentColor()
    {
        return text.color;
    }

    internal override void SetColor(Color obj)
    {
        text.color = obj;
    }
}
