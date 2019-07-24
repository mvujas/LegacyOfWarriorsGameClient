using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(InputField), typeof(ImageColorTransitionable))]
public class FancyInputField : MonoBehaviourWithAddOns
{
    [SerializeField]
    private PositionTransitionable placeholderPositionTransitionable = null;
    [SerializeField]
    private TextColorTransitionable placeholderColorTransitionable = null;

    private InputField inputField = null;
    private bool isInFocus = false;

    private ImageColorTransitionable imageColorTransitionable = null;

    private void Awake()
    {
        if(placeholderPositionTransitionable == null)
        {
            throw new ArgumentNullException(nameof(placeholderPositionTransitionable));
        }
        imageColorTransitionable = GetComponent<ImageColorTransitionable>();
        inputField = GetComponent<InputField>();
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

    public void OnFieldValueChange()
    {
        if(inputField.text == "")
        {
            placeholderPositionTransitionable.GoToStart();
        }
        else
        {
            placeholderPositionTransitionable.GoToEnd();
        }
    }

    private void OnFocusLost()
    {
        imageColorTransitionable.ChangeColorToInitial();
        placeholderColorTransitionable.ChangeColorToInitial();
    }

    private void OnFocusEntered()
    {
        imageColorTransitionable.ChangeColorToEnd();
        placeholderColorTransitionable.ChangeColorToEnd();
    }

}
