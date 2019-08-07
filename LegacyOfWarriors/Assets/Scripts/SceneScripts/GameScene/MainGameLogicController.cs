using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainGameLogicController : MonoBehaviourWithAddOns
{
    [SerializeField]
    private StartingGamePanelDisabler panelDisabler = null;
    [SerializeField]
    private HandController playersHandController = null;
    [SerializeField]
    private BoardSideController playersBoardSideController = null;
    [SerializeField]
    private BoardSideController enemiesBoardSideController = null;
    [SerializeField]
    private PlayerDataController playersDataController = null;
    [SerializeField]
    private PlayerDataController enemiesDataController = null;


    private MutablePassiveRequestMapper m_requestMapper = new MutablePassiveRequestMapper();

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
    }

    private void Awake()
    {
        CheckSerializedFieldsForNull();
    }

    private void Start()
    {
        
    }
}
