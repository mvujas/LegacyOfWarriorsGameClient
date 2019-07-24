using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Delegates;

public abstract class ColorTransitionable : MonoBehaviourWithAddOns
{
    [SerializeField]
    private Color endColor = Color.black;
    [SerializeField]
    private float m_animationDuration = 0.1f;
    public float AnimationDuration
    {
        get => m_animationDuration;
        set => m_animationDuration = Mathf.Max(0.01f, m_animationDuration);
    }

    private void OnValidate()
    {
        AnimationDuration = m_animationDuration;
    }

    private static readonly TransitionFunction<Color> linear = (t, b, c, d) =>
    {
        return c * t / d + b;
    };

    private Color initialColor;

    private float fullColorDifference;

    internal abstract Color GetCurrentColor();
    internal abstract void SetColor(Color obj);

    protected virtual void Awake()
    {
        initialColor = GetCurrentColor();
        fullColorDifference = ColorDifference(initialColor, endColor);
    }


    private float ColorDifference(Color color1, Color color2)
    {
        return ((Vector4)color1 - (Vector4)color2).magnitude;
    }

    private void AnimateChangeToTargetColor(Color targetColor, Supplier<bool> exitPredicate)
    {
        Color startingColor = GetCurrentColor();
        float pathRatio = ColorDifference(startingColor, targetColor) / fullColorDifference;
        if (pathRatio < 0.01f)
        {
            return;
        }
        float time = pathRatio * AnimationDuration;
        Color change = targetColor - startingColor;
        PlayTransition<Color>(linear, time, startingColor, change, SetColor, exitPredicate);
    }

    public void ChangeColorToInitial(Supplier<bool> exitPredicate = null)
    {
        AnimateChangeToTargetColor(initialColor, exitPredicate);
    }

    public void ChangeColorToEnd(Supplier<bool> exitPredicate = null)
    {
        AnimateChangeToTargetColor(endColor, exitPredicate);
    }

}

