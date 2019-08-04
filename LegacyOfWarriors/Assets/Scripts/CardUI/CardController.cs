using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CardController : MonoBehaviourWithAddOns
{
    [SerializeField]
    private Text cardNameText = null;
    [SerializeField]
    private Text costText = null;
    [SerializeField]
    private Text attackText = null;
    [SerializeField]
    private Text healthText = null;

    #region PROPERTIES
    private string m_cardName;
    public string CardName
    {
        get => m_cardName;
        set
        {
            m_cardName = value;
            cardNameText.text = value;
        }
    }

    private int m_cost;
    public int Cost
    {
        get => m_cost;
        set
        {
            m_cost = value;
            costText.text = value.ToString();
        }
    }

    private int m_attack;
    public int Attack
    {
        get => m_attack;
        set
        {
            m_attack = value;
            attackText.text = value.ToString();
        }
    }

    private int m_health;
    public int Health
    {
        get => m_health;
        set
        {
            m_health = value;
            healthText.text = value.ToString();
        }
    }
    #endregion

    private void Awake()
    {
        if (cardNameText == null || costText == null || attackText == null || healthText == null)
        {
            throw new ArgumentNullException("Card texts are not set");
        }
    }

    private void Start()
    {
        CardName = "Cica glisa";
        Cost = 1;
        Health = 2;
        Attack = 5;
    }
}
