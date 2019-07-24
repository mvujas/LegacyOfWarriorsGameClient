using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Delegates;

public class PositionTransitionable : MonoBehaviourWithAddOns
{
    [SerializeField]
    private Transform endPositionTransform = null;
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

    private Vector3 startPosition;
    private Vector3 endPosition;

    private float fullPathLength;

    private static readonly TransitionFunction<Vector3> quadraticEaseIn = (t, b, c, d) =>
    {
        t /= d;
        return c * (t * t) + b;
    };

    private void Awake()
    {
        startPosition = transform.localPosition;
        endPosition = endPositionTransform.localPosition;
        fullPathLength = (endPosition - startPosition).magnitude;
    }

    private float PathLengthToReferencePoint(Vector3 referencePoint)
    {
        return (transform.localPosition - referencePoint).magnitude;
    }

    private void SetPosition(Vector3 newPosition)
    {
        transform.localPosition = newPosition;
    }

    private void AnimateMovementToTargetPoint(Vector3 targetPosition, Supplier<bool> exitPredicate)
    {
        Vector3 animationStartingPosition = transform.localPosition;
        float pathRatio = PathLengthToReferencePoint(targetPosition) / fullPathLength;
        if(pathRatio < 0.01f)
        {
            return;
        }
        float time = pathRatio * AnimationDuration;
        Vector3 change = targetPosition - animationStartingPosition;
        PlayTransition<Vector3>(quadraticEaseIn, time, animationStartingPosition, change, SetPosition, exitPredicate);
    }

    public void GoToEnd(Supplier<bool> exitPredicate = null)
    {
        AnimateMovementToTargetPoint(endPosition, exitPredicate);
    }

    public void GoToStart(Supplier<bool> exitPredicate = null)
    {
        AnimateMovementToTargetPoint(startPosition, exitPredicate);
    }
}
