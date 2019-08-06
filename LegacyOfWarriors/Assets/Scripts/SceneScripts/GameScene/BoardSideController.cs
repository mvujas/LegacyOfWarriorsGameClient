using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Remote.InGameObjects;
using System.Linq;

public class BoardSideController : MonoBehaviourWithAddOns
{
    [SerializeField]
    private CardController[] cardPlaceholders = { };

    private CardInGame[] cardInGameArr = null;

    private void Awake()
    {
        cardPlaceholders = cardPlaceholders.Where(cp => cp != null).ToArray();

        cardInGameArr = new CardInGame[cardPlaceholders.Length];
        foreach (var placeholder in cardPlaceholders)
        {
            placeholder.gameObject.SetActive(false);
        }
    }

    public bool AddCard(CardInGame cardInGame)
    {
        for(int i = 0; i < this.cardInGameArr.Length; i++)
        {
            if(cardInGameArr[i] == null)
            {
                cardPlaceholders[i].ReplicateStats(cardInGame);
                cardInGameArr[i] = cardInGame;
                cardPlaceholders[i].gameObject.SetActive(true);
                return true;
            }
        }
        return false;
    }

    public bool RemoveCard(int cardInGameId)
    {
        for (int i = 0; i < this.cardInGameArr.Length; i++)
        {
            var cardInGame = cardInGameArr[i];
            if (cardInGame != null && cardInGame.InGameId == cardInGameId)
            {
                cardPlaceholders[i].ReplicateStats(cardInGame);
                cardInGameArr[i] = null;
                cardPlaceholders[i].gameObject.SetActive(false);
                return true;
            }
        }
        return false;
    }

    #region DEBUGGING
    
    private void Start()
    {
        Card card = new Card(1, "Karta1", "karta1", 5, 1, 2);
        CardInGame cardInGame1 = new CardInGame
        {
            InGameId = 0
        };

        CardInGame cardInGame2 = new CardInGame
        {
            InGameId = 1
        };

        cardInGame1.SetCard(card);
        cardInGame2.SetCard(card);

        AddCard(cardInGame1);
        AddCard(cardInGame2);
    }

    private int count = 0;
    private void Update()
    {
        if (count++ == 100)
        {
            Debug.Log("Removing: " + RemoveCard(1));
        }

        if (count == 200)
        {
            Card card = new Card(3, "Karta2", "karta2", 5, 1, 2);
            CardInGame cardInGame = new CardInGame
            {
                InGameId = 3
            };

            cardInGame.SetCard(card);

            AddCard(cardInGame);
            AddCard(cardInGame);
            AddCard(cardInGame);
            AddCard(cardInGame);
            AddCard(cardInGame);
            AddCard(cardInGame);
        }
    }

    #endregion
}
