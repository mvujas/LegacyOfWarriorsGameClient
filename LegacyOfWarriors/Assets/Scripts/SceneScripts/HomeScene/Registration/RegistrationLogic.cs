using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Remote;
using UnityEngine.UI;

public class RegistrationLogic : TemporarySimpleGUIComponent
{
    [SerializeField]
    private Text infoText = null;
    [SerializeField]
    private InputField usernameField = null;
    [SerializeField]
    private InputField passwordField = null;
    [SerializeField]
    private InputField repeatedPasswordField = null;

    [SerializeField]
    private InterSceneMultiGUIController interSceneMultiGUIController = null;

    protected override RemoteRequestMapper GetRemoteRequestMapper()
    {
        return null;
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        ResetInfoText();
        ResetForm();
        base.Hide();
    }

    private void Awake()
    {
        if (usernameField == null || passwordField == null || repeatedPasswordField == null)
        {
            throw new ArgumentException("Registration fields are not initialized");
        }
        if (interSceneMultiGUIController == null)
        {
            throw new ArgumentException("No GUI controller set");
        }
    }

    private void ResetForm()
    {
        if(usernameField != null)
        {
            usernameField.text = "";
        }
        if (passwordField != null)
        {
            passwordField.text = "";
        }
        if (repeatedPasswordField != null)
        {
            repeatedPasswordField.text = "";
        }
    }

    private void ResetInfoText()
    {
        if (infoText != null)
        {
            infoText.text = null;
        }
    }

    private void ShowErrorMessage(string errorMessage)
    {
        if (infoText != null)
        {
            infoText.color = Color.red;
            infoText.text = errorMessage;
        }
    }

    private void ShowSuccessMessage(string successMessage)
    {
        if (infoText != null)
        {
            infoText.color = Color.green;
            infoText.text = successMessage;
        }
    }

    public void HeadToLogin()
    {
        interSceneMultiGUIController.Show("LoginScreen");
    }
}
