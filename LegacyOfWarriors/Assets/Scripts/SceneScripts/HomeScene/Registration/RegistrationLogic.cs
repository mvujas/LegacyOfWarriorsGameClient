using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Remote;
using UnityEngine.UI;
using Remote.Implementation;

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

    private MutablePassiveRequestMapper m_mapper = new MutablePassiveRequestMapper();
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
        infoText?.SetRegularText("");
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

        m_mapper.AddHandlerAction<RegistrationResponse>(HandleRegistrationReponse);
    }

    private void ResetForm()
    {
        usernameField.text = "";
        passwordField.text = "";
        repeatedPasswordField.text = "";
    }

    private void ShowError(string message)
    {
        infoText?.SetErrorText("*** GREŠKA ***\n" + message);
    }

    private void HandleRegistrationReponse(RegistrationResponse response)
    {
        if(response.Successfulness)
        {
            infoText?.SetSuccessText("Uspešno ste se registrovali");
            ResetForm();
        }
        else
        {
            ShowError(response.Message);
        }
    }

    public void HeadToLogin()
    {
        interSceneMultiGUIController.Show("LoginScreen");
    }

    public void HandleRegistrationClick()
    {
        string username = usernameField.text.Trim(),
            password = passwordField.text.Trim(),
            repeatedPassword = repeatedPasswordField.text.Trim();

        if(password != repeatedPassword)
        {
            ShowError("Unete lozinke se ne slažu");
            return;
        }

        globalReference.GameClient.Send(new RegistrationRequest { Username = username, Password = password });
    }
}
