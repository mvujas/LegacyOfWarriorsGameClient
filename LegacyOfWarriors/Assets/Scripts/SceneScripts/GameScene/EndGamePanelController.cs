using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndGamePanelController : MonoBehaviourWithAddOns
{
    [SerializeField]
    private GameObject victoryImage = null;
    [SerializeField]
    private GameObject defeatImage = null;
    [SerializeField]
    private Animator animator = null;

    private void Awake()
    {
        if(victoryImage == null)
        {
            throw new ArgumentNullException(nameof(victoryImage));
        }
        if (defeatImage == null)
        {
            throw new ArgumentNullException(nameof(defeatImage));
        }
        if (animator == null)
        {
            throw new ArgumentNullException(nameof(animator));
        }
    }

    public void Show(bool isVictory)
    {
        GameObject imageToDisable = isVictory ? defeatImage : victoryImage;
        imageToDisable.transform.localScale = Vector3.zero;

        animator.SetBool("IsMatchOver", true);
    }
}
