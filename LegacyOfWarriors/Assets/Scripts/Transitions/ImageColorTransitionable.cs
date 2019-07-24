using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Utils.Delegates;

[RequireComponent(typeof(Image))]
public class ImageColorTransitionable : ColorTransitionable
{
    private Image image = null;

    protected override void Awake()
    {
        image = GetComponent<Image>();
        base.Awake();
    }

    internal override void SetColor(Color obj)
    {
        image.color = obj;
    }

    internal override Color GetCurrentColor()
    {
        return image.color;
    }
}
