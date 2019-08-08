using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Remote.Implementation;
using Remote.InGameObjects;

public class MainGameLogicController : MonoBehaviourWithAddOns
{
    private class PlayerControllersContainer
    {
        public BoardSideController boardSideController;
        public PlayerDataController dataController;
    }
    private PlayerControllersContainer[] playersControllers = new PlayerControllersContainer[2];

    public StartingGamePanelDisabler panelDisabler = null;
    public HandController playersHandController = null;
    public BoardSideController playersBoardSideController = null;
    public BoardSideController enemiesBoardSideController = null;
    public PlayerDataController playersDataController = null;
    public PlayerDataController enemiesDataController = null;
    public EndTurnButtonStateController endTurnButton = null;

    [SerializeField]
    private RequestMapperContainer mapperContainer = null;

    public bool IsPlayersTurn { get; private set; } = false;
    public int PlayerIndex { get; private set; }

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
        mapperContainer.RequestMapper.AddHandlerAction<NewTurnNotification>(HandleNewTurnNotification);
        mapperContainer.RequestMapper.AddHandlerAction<EndTurnResponse>(HandleEndTurnResponse);
    }

    private void Awake()
    {
        CheckSerializedFieldsForNull();
        SetHandlers();
    }

    private void Start()
    {
        /*panelDisabler.Disable();
        ExecuteAfterDelay(() => endTurnButton.ActiveState = true, 1f);*/
    }

    private void PrepareCardInGame(CardInGame cardInGame)
    {
        if (cardInGame == null)
        {
            return;
        }
        cardInGame.Card = globalReference.CardList.GetCardById(cardInGame.CardId);
        if (cardInGame.Card == null)
        {
            throw new Exception("Trying to get card that doesn't exist in card list");
        }
    }

    public void HandleEndTurn()
    {
        globalReference.GameClient.Send(new EndTurnRequest());
    }

    private void SwitchTurn(bool isMyTurn)
    {
        IsPlayersTurn = isMyTurn;
        endTurnButton.ActiveState = isMyTurn;
    }

    #region HANDLERS

    private void HandleStartingGameState(StartingUserGameState startingState)
    {
        panelDisabler.Disable();

        PlayerIndex = startingState.PlayerIndex;
        int initHandSize = startingState.StartingDeck.Count;
        for (int i = 0; i < 2; i++)
        {
            playersControllers[i] = new PlayerControllersContainer
            {
                boardSideController = (i == PlayerIndex) ? playersBoardSideController : enemiesBoardSideController,
                dataController = (i == PlayerIndex) ? playersDataController : enemiesDataController
            };
            playersControllers[i].dataController.HandSize = initHandSize;
            playersControllers[i].dataController.DeckSize -= initHandSize;
        }

        foreach (var cardInGame in startingState.StartingDeck)
        {
            PrepareCardInGame(cardInGame);
            playersHandController.AddCard(cardInGame);
        }
    }

    private void HandleNewTurnNotification(NewTurnNotification newTurnNotification)
    {
        var idOfPlayerWithTurn = newTurnNotification.PlayerIndex;
        if (idOfPlayerWithTurn == PlayerIndex)
        {
            switch (newTurnNotification.DrawingOutcome)
            {
                case CardDrawingOutcome.SUCCESSFUL:
                {
                    var card = newTurnNotification.DrawnCard;
                    PrepareCardInGame(card);
                    playersHandController.AddCard(card);
                    break;
                }
            }
        }

        var currentPlayerControllers = playersControllers[idOfPlayerWithTurn];
        switch (newTurnNotification.DrawingOutcome)
        {
            case CardDrawingOutcome.SUCCESSFUL:
            {
                currentPlayerControllers.dataController.HandSize++;
                currentPlayerControllers.dataController.DeckSize--;
                break;
            }
            case CardDrawingOutcome.FULL_HAND:
            {
                currentPlayerControllers.dataController.DeckSize--;
                break;
            }
        }
                    
        currentPlayerControllers.dataController.Mana = newTurnNotification.Mana;
        SwitchTurn(idOfPlayerWithTurn == PlayerIndex);
    }

    private void HandleEndTurnResponse(EndTurnResponse response)
    {
        Debug.Log("Odgovor na zahtev za kraj poteza: \n" +
            $"Uspesnost: {response.Successfulness}, poruka: {response.Message}");
    }

    #endregion
}
