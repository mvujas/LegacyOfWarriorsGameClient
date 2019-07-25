using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviourWithAddOns
{
    [SerializeField]
    private SettingsMenuPanel parentPanel = null;

    public void QuitGame()
    {
        Application.Quit();
    }
}
