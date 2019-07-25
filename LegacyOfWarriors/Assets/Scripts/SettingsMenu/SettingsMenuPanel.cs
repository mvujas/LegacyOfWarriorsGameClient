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

    private void Awake()
    {
        imageColorTransitionable = GetComponent<ImageColorTransitionable>();
    }

    public void Show()
    {
        if(!gameObject.activeSelf)
        {
            settingsMenu.SetActive(false);
            gameObject.SetActive(true);
            imageColorTransitionable.ChangeColorToEnd();
            ExecuteAfterDelay(() => settingsMenu.SetActive(true), imageColorTransitionable.AnimationDuration);
        }
    }

    public void Hide()
    {
        if (gameObject.activeSelf)
        {
            settingsMenu.SetActive(false);
            imageColorTransitionable.ChangeColorToInitial();
            ExecuteAfterDelay(() =>
            {
                gameObject.SetActive(false);
            }, imageColorTransitionable.AnimationDuration);
        }
    }


}
