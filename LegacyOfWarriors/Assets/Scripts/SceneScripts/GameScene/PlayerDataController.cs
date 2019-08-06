using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerDataController : MonoBehaviourWithAddOns
{
    [SerializeField]
    private Text deckSizeText;
    [SerializeField]
    private Text handSizeText;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text manaText;

    #region PROPERTIES

    public int DeckSize
    {
        set
        {
            deckSizeText.text = value.ToString();
        }
    }

    public int HandSize
    {
        set
        {
            handSizeText.text = value.ToString();
        }
    }

    public int Health
    {
        set
        {
            healthText.text = value.ToString();
        }
    }

    public int Mana
    {
        set
        {
            manaText.text = value.ToString();
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
        DeckSize = 5;
        HandSize = 10;
        Health = 0;
        Mana = 15;
    }
}
