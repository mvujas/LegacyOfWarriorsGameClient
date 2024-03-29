﻿using System.Collections;
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

public enum CardOwner
{
    PLAYER,
    ENEMY
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
    [SerializeField]
    private Image cardImage = null;
    [SerializeField]
    private GameObject cardHighlighter = null;

    public CardOwner cardOwner = CardOwner.PLAYER;

    public ClientSideCardPlace cardPlace = ClientSideCardPlace.NONE;

    #region PROPERTIES
    public int CardInGameId { get; set; }
    public int LastAttackingTurn { get; set; }

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

    private string m_imageName;
    public string ImageName
    {
        get => m_imageName;
        set
        {
            m_imageName = value;
            globalReference.SpriteCatalogue.GetSprite(value, out Sprite sprite);
            cardImage.sprite = sprite;
        }
    }

    private bool m_isHighlighted;
    public bool IsHighlighted
    {
        get => m_isHighlighted;
        set
        {
            m_isHighlighted = value;
            cardHighlighter.SetActive(m_isHighlighted);
        }
    }
    #endregion

    private void Awake()
    {
        if (cardNameText == null || costText == null || attackText == null || healthText == null)
        {
            throw new ArgumentNullException("Card texts are not set");
        }
        if (cardImage == null)
        {
            throw new ArgumentNullException("Card image is not set");
        }
    }

    public void ReplicateStats(CardInGame cardInGame)
    {
        var card = cardInGame.Card;
        ImageName = card.ClientSideImage;
        CardInGameId = cardInGame.InGameId;
        CardName = card.Name;
        Cost = cardInGame.Cost;
        Attack = cardInGame.Attack;
        Health = cardInGame.Health;
        LastAttackingTurn = cardInGame.LastAttackingTurn;
    }

    public void ReplicateStats(CardController cardController)
    {
        CardInGameId = cardController.CardInGameId;
        ImageName = cardController.ImageName;
        CardName = cardController.CardName;
        Cost = cardController.Cost;
        Attack = cardController.Attack;
        Health = cardController.Health;
        LastAttackingTurn = cardController.LastAttackingTurn;
    }
}
