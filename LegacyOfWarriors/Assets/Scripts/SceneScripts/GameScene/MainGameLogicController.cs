using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Remote.Implementation;
using Remote.InGameObjects;

public class MainGameLogicController : MonoBehaviourWithAddOns
{
    public StartingGamePanelDisabler panelDisabler = null;
    public HandController playersHandController = null;
    public BoardSideController playersBoardSideController = null;
    public BoardSideController enemiesBoardSideController = null;
    public PlayerDataController playersDataController = null;
    public PlayerDataController enemiesDataController = null;
    public EndTurnButtonStateController endTurnButton = null;

    [SerializeField]
    private RequestMapperContainer mapperContainer = null;

    private void CheckSerializedFieldsForNull()
    {
        if (panelDisabler == null)
        {
            throw new ArgumentNullException(nameof(panelDisabler));
        }
        if (playersHandController == null)
        {
            throw new ArgumentNullException(nameof(playersHandController));
        }
        if (playersBoardSideController == null)
        {
            throw new ArgumentNullException(nameof(playersBoardSideController));
        }
        if (enemiesBoardSideController == null)
        {
            throw new ArgumentNullException(nameof(enemiesBoardSideController));
        }
        if (playersDataController == null)
        {
            throw new ArgumentNullException(nameof(playersDataController));
        }
        if (enemiesDataController == null)
        {
            throw new ArgumentNullException(nameof(enemiesDataController));
        }
        if (endTurnButton == null)
        {
            throw new ArgumentNullException(nameof(endTurnButton));
        }

        if (mapperContainer == null)
        {
            throw new ArgumentNullException(nameof(mapperContainer));
        }
    }

    private void SetHandlers()
    {
        mapperContainer.RequestMapper.AddHandlerAction<StartingUserGameState>(HandleStartingGameState);
    }

    private void Awake()
    {
        CheckSerializedFieldsForNull();
        //SetHandlers();
    }

    private void Start()
    {
        panelDisabler.Disable();
        ExecuteAfterDelay(() => endTurnButton.ActiveState = true, 1f);
    }

    private void HandleStartingGameState(StartingUserGameState startingState)
    {
        panelDisabler.Disable();

        int initHandSize = startingState.StartingDeck.Count;
        playersDataController.HandSize = initHandSize;
        playersDataController.DeckSize -= initHandSize;
        enemiesDataController.HandSize = initHandSize;
        enemiesDataController.DeckSize -= initHandSize;
        foreach (var cardInGame in startingState.StartingDeck)
        {
            PrepareCardInGame(cardInGame);
            playersHandController.AddCard(cardInGame);
        }
    }

    private void PrepareCardInGame(CardInGame cardInGame)
    {
        if (cardInGame == null)
        {
            return;
        }
        cardInGame.Card = globalReference.CardList.GetCardById(cardInGame.CardId);
        if(cardInGame.Card == null)
        {
            throw new Exception("Trying to get card that doesn't exist in card list");
        }
    }
}
