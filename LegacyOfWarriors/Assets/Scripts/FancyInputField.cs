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
    private bool isFieldActive = false;

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
            SetFieldStateToDeactivated();
        }
        else
        {
            SetFieldStateToActivated();
        }
    }

    private void OnFocusLost()
    {
        if(inputField.text == "")
        {
            SetFieldStateToDeactivated();
        }
    }

    private void OnFocusEntered()
    {
        SetFieldStateToActivated();
    }

    private void SetFieldStateToActivated()
    {
        if(isFieldActive)
        {
            return;
        }
        isFieldActive = true;
        imageColorTransitionable.ChangeColorToEnd();
        placeholderColorTransitionable.ChangeColorToEnd();
        placeholderPositionTransitionable.GoToEnd();
    }

    private void SetFieldStateToDeactivated()
    {
        if(!isFieldActive)
        {
            return;
        }
        isFieldActive = false;
        imageColorTransitionable.ChangeColorToInitial();
        placeholderColorTransitionable.ChangeColorToInitial();
        placeholderPositionTransitionable.GoToStart();
    }

}
