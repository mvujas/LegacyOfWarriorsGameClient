using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Utils.GameLogicUtils;
using Remote.InGameObjects;

public enum ClientSideCardPlace
{
    DECK,
    HAND,
    FIELD,
    NONE
}

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

    public ClientSideCardPlace cardPlace = ClientSideCardPlace.NONE;

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

    public void ReplicateStats(CardInGame cardInGame)
    {
        var card = cardInGame.Card;
        CardName = card.Name;
        Cost = cardInGame.Cost;
        Attack = cardInGame.Attack;
        Health = cardInGame.Health;
    }

    public void ReplicateStats(CardController cardController)
    {
        CardName = cardController.CardName;
        Cost = cardController.Cost;
        Attack = cardController.Attack;
        Health = cardController.Health;
    }
}
