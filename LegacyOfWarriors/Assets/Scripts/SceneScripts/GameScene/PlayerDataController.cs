using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerDataController : MonoBehaviourWithAddOns
{
    [SerializeField]
    private Text deckSizeText = null;
    [SerializeField]
    private Text handSizeText = null;
    [SerializeField]
    private Text healthText = null;
    [SerializeField]
    private Text manaText = null;

    #region PROPERTIES

    private int m_deckSize;
    public int DeckSize
    {
        get => m_deckSize;
        set
        {
            m_deckSize = Math.Max(0, value);
            deckSizeText.text = m_deckSize.ToString();
        }
    }

    private int m_handSize;
    public int HandSize
    {
        get => m_handSize;
        set
        {
            m_handSize = Math.Max(0, value);
            handSizeText.text = m_handSize.ToString();
        }
    }

    private int m_health;
    public int Health
    {
        get => m_health;
        set
        {
            m_health = value;
            healthText.text = m_health.ToString();
        }
    }

    private int m_mana;
    public int Mana
    {
        get => m_mana;
        set
        {
            m_mana = Math.Max(0, value);
            manaText.text = m_mana.ToString();
        }
    }

    #endregion

    private void Awake()
    {
        if(deckSizeText == null || handSizeText == null || healthText == null || manaText == null)
        {
            throw new ArgumentNullException("Player Data texts not set");
        }
    }


    private void Start()
    {
        /*
        DeckSize = 5;
        HandSize = 10;
        Health = 0;
        Mana = 15;
        */
    }
}
