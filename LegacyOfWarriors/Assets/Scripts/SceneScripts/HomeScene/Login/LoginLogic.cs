﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Remote;
using ClientUtils;
using Remote.Implementation;
using UnityEngine.UI;

public class LoginLogic : TemporarySimpleGUIComponent
{
    [SerializeField]
    private Text infoText = null;
    [SerializeField]
    private InputField usernameField = null;
    [SerializeField]
    private InputField passwordField = null;

    [SerializeField]
    private InterSceneMultiGUIController interSceneMultiGUIController = null;

    private GameClient m_gameClient = null;

    private RemoteRequestMapper m_mapper = null;
    protected override RemoteRequestMapper GetRemoteRequestMapper()
    {
        return m_mapper;
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
        if(usernameField == null || passwordField == null)
        {
            throw new ArgumentException("Login fields are not initialized");
        }

        if(interSceneMultiGUIController == null)
        {
            throw new ArgumentException("No GUI controller set");
        }

        m_mapper = new LoginRequestMapper(
            onLoginSuccessful: () => RunInMainThread(() => OnSuccessfulLogin()),
            onLoginFailed: message => RunInMainThread(() => OnUnsuccessfulLogin(message))
        );
    }

    private void ResetForm()
    {
        if(usernameField != null)
        {
            usernameField.text = "";
        }
        if(passwordField != null)
        {
            passwordField.text = "";
        }
    }

    private void ResetInfoText()
    {
        if(infoText != null)
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

    private void OnSuccessfulLogin()
    {
        ShowSuccessMessage("Uspesno ste se ulogovali!");
    }

    private void OnUnsuccessfulLogin(string errorMessage)
    {
        ShowErrorMessage(" *** GRESKA *** \n" + errorMessage);
    }

    private void Start()
    {
        m_gameClient = globalReference.GameClient;
    }

    public void TryToLogin()
    {
        string username = usernameField.text.Trim();
        string password = passwordField.text.Trim();
        m_gameClient.Send(new LoginRequest
        {
            Username = username,
            Password = password
        });
    }

    public void HeadToRegistration()
    {
        interSceneMultiGUIController.Show("RegistrationScreen");
    }
}
