using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class FancyInputField : MonoBehaviourWithAddOns
{
    [SerializeField]
    private float animationDuration = 1f;
    [SerializeField]
    private Text placeholder = null;
    [SerializeField]
    private Transform placeholderInFocusPosition = null;

    private Vector3 focusPosition;
    private Vector3 outOfFocusPosition;

    private void OnValidate()
    {
        animationDuration = Mathf.Max(.01f, animationDuration);
    }

    private InputField inputField = null;
    private bool isInFocus = false;
    private bool isUIInFocus = false;
    private TransitionFunction<Vector3> nesto = (t, b, c, d) =>
    {
        t /= d;
        return c * t * t + b;
    };
    private float fullPathLength;

    private void Awake()
    {
        outOfFocusPosition = placeholder.transform.position;
        focusPosition = placeholderInFocusPosition.position;
        fullPathLength = (outOfFocusPosition - focusPosition).magnitude;
        inputField = GetComponent<InputField>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        CheckFocusChange();
    }

    private void CheckFocusChange()
    {
        if (inputField.isFocused != isInFocus)
        {
            isInFocus = inputField.isFocused;
            if (isInFocus)
            {
                OnFocusEntered();
            }
            else
            {
                OnFocusLost();
            }
        }
    }

    private void SetPlaceholderPosition(Vector3 newPosition)
    {
        placeholder.transform.position = newPosition;
    }

    private float PathLengthFromPlaceholderToPoint(Vector3 point)
    {
        return (point - placeholder.transform.position).magnitude;
    }

    private void AnimatePlaceholderMovementBetweenTargetPoints(Vector3 end)
    {
        Vector3 start = placeholder.transform.position;
        float pathRatio = PathLengthFromPlaceholderToPoint(end) / fullPathLength;
        float time = pathRatio * animationDuration;
        Vector3 change = end - start;
        PlayTransition<Vector3>(nesto, time, start, change, SetPlaceholderPosition);
    }

    private void OnFocusLost()
    {
        if(inputField.text.Length == 0)
        {
            isUIInFocus = false;
            AnimatePlaceholderMovementBetweenTargetPoints(outOfFocusPosition);
        }
        
    }

    private void OnFocusEntered()
    {
        if (!isUIInFocus)
        {
            AnimatePlaceholderMovementBetweenTargetPoints(focusPosition);
        }
        isUIInFocus = true;
        
    }


}
