using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Animator), typeof(Button))]
public class EndTurnButtonStateController : MonoBehaviourWithAddOns
{
    private Animator animator = null;
    private Button button = null;

    private bool m_activeState;
    public bool ActiveState
    {
        get => m_activeState;
        set
        {
            if(m_activeState != value)
            {
                m_activeState = value;
                button.enabled = value;
                animator.SetBool("IsActive", value);
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();

        m_activeState = animator.GetBool("IsActive");
        button.enabled = m_activeState;
    }
}
