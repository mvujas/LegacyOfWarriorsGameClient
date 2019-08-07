using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class StartingGamePanelDisabler : MonoBehaviourWithAddOns
{
    private Animator animator = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Disable()
    {
        animator.SetBool("AreAllPlayersConnected", true);
    }

    private void Update()
    {
        if(gameObject.activeInHierarchy)
        {
            var animState = animator.GetCurrentAnimatorStateInfo(0);
            if(animState.IsName("PanelFadeOut") && animState.normalizedTime > 1)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
