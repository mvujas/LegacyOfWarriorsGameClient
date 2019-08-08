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
        SetHandlers();
    }

    private void Start()
    {
        
    }

    private void HandleStartingGameState(StartingUserGameState startingState)
    {
        panelDisabler.Disable();

        Debug.Log("Starting state:");
        Debug.Log(startingState.PlayerIndex);
        Debug.Log(startingState.StartingDeck);
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
