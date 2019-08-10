using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ProjectLevelConfig;
using Utils.DataTypes;
using Utils.Delegates;
using Remote.InGameObjects;

public class HandController : MonoBehaviourWithAddOns
{
    [SerializeField]
    private GameObject handStringPrefab = null;
    [SerializeField]
    private float maxTotalSideAngle = 30;
    [SerializeField]
    private float maxSingleAngle = 20;

    private ObjectPool<HandStringController> m_handStringControllerPool = null;
    private LinkedList<CardStringWrapper> m_cards = new LinkedList<CardStringWrapper>();

    private class CardStringWrapper
    {
        public CardInGame cardInGame;
        public HandStringController handString;
    }

    private void Awake()
    {
        if (handStringPrefab == null)
        {
            throw new ArgumentNullException(nameof(handStringPrefab));
        }

        Supplier<HandStringController> handStringSupplier = () =>
        {
            GameObject @object = Instantiate(handStringPrefab, Vector3.zero, Quaternion.identity, transform);
            @object.transform.localPosition = Vector3.zero;
            HandStringController stringController = @object.GetComponent<HandStringController>();
            if (stringController == null)
            {
                throw new ArgumentNullException("Prefab doesn't have appropriate controller attached");
            }
            @object.SetActive(false);
            return stringController;
        };

        m_handStringControllerPool = new ObjectPool<HandStringController>(handStringSupplier, 5, true);
    }

    private void ReajustStrings()
    {
        int numberOfStrings = m_cards.Count;
        if(numberOfStrings == 0)
        {
            return;
        }
        if(numberOfStrings == 1)
        {
            var handString = m_cards.First.Value.handString;
            handString.SetZRotation(0);
            return;
        }

        int interAngles = numberOfStrings - 1;
        float totalSideAngle = Mathf.Min(maxTotalSideAngle, interAngles * maxSingleAngle / 2);
        float singleAngle = 2 * totalSideAngle / interAngles;

        int counter = 0;
        foreach(var cardContainer in m_cards)
        {
            var handString = cardContainer.handString;
            handString.SetZRotation(totalSideAngle - counter * singleAngle);
            handString.transform.SetSiblingIndex(counter);
            counter++;
        }
    }

    public void AddCard(CardInGame cardInGame, bool autoReajust = true)
    {
        var handString = m_handStringControllerPool.GetObject();
        m_cards.AddLast(new CardStringWrapper
        {
            cardInGame = cardInGame,
            handString = handString
        });
        handString.SetCardStats(cardInGame);
        handString.gameObject.SetActive(true);
        if (autoReajust)
        {
            ReajustStrings();
        }
    }

    public bool RemoveCard(int cardInGameId, bool autoReajust = true)
    {
        for(var iter = m_cards.First; iter != null; iter = iter.Next)
        {
            var cardContainer = iter.Value;
            if (cardContainer.cardInGame.InGameId == cardInGameId)
            {
                m_cards.Remove(iter);
                RemoveContainer(cardContainer);
                if (autoReajust)
                {
                    ReajustStrings();
                }
                return true;
            }
        }
        return false;
    }

    private void RemoveContainer(CardStringWrapper container)
    {
        var handString = container.handString;
        handString.gameObject.SetActive(false);
        m_handStringControllerPool.ReleaseObject(handString);
    }

    public IEnumerable<CardController> GetAllCardControllers()
    {
        foreach(var cardStringWrapper in m_cards)
        {
            var cardController = cardStringWrapper.handString.cardController;
            if(cardController != null)
            {
                yield return cardController;
            }
        }
    }

    /*
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

        AddCard(cardInGame1, false);
        AddCard(cardInGame2, false);
        ReajustStrings();
    }

    private int count = 0;
    private void Update()
    {
        if(count ++ == 150)
        {
            Debug.Log("Removing: " + RemoveCard(1));
        }
    }
    #endregion
    */
}
