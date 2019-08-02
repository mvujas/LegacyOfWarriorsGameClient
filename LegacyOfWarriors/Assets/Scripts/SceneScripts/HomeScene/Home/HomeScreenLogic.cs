using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Remote;
using UnityEngine.UI;

public class HomeScreenLogic : TemporarySimpleGUIComponent
{
    [SerializeField]
    private UserInfoContainer userInfoContainer = null;
    [SerializeField]
    private Text userInfoText = null;

    protected override RemoteRequestMapper GetRemoteRequestMapper()
    {
        return null;
    }

    public override void Show()
    {
        PrepareHomeScreen();
        base.Show();
    }

    private void PrepareHomeScreen()
    {
        userInfoText.text = "Dobrodošao!\nKorisnik: " + userInfoContainer.UserInfo.Username;
    }

    private void Awake()
    {
        if(userInfoContainer == null)
        {
            throw new ArgumentNullException(nameof(userInfoContainer));
        }
        if(userInfoText == null)
        {
            throw new ArgumentNullException(nameof(userInfoText));
        }
    }
}
