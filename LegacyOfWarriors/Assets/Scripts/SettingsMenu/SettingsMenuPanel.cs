using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(ImageColorTransitionable))]
public class SettingsMenuPanel : MonoBehaviourWithAddOns
{
    [SerializeField]
    private GameObject settingsMenu = null;
    private ImageColorTransitionable imageColorTransitionable = null;

    private bool isShown;

    private void Awake()
    {
        imageColorTransitionable = GetComponent<ImageColorTransitionable>();
        isShown = gameObject.activeInHierarchy;
    }

    public void Show()
    {
        if(!isShown)
        {
            isShown = true;
            settingsMenu.SetActive(false);
            gameObject.SetActive(true);
            imageColorTransitionable.ChangeColorToEnd();
            ExecuteAfterDelay(() => settingsMenu.SetActive(true), imageColorTransitionable.AnimationDuration);
        }
    }

    public void Hide()
    {
        if (isShown)
        {
            isShown = false;
            settingsMenu.SetActive(false);
            imageColorTransitionable.ChangeColorToInitial();
            ExecuteAfterDelay(() =>
            {
                gameObject.SetActive(false);
            }, imageColorTransitionable.AnimationDuration);
        }
    }


}
