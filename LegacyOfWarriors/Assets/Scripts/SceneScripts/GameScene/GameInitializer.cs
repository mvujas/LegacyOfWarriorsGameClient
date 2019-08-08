using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Remote.Implementation;
using UnityEngine.UI;

public class GameInitializer : MonoBehaviourWithAddOns
{
    [SerializeField]
    private RequestMapperContainer mapperContainer = null;
    [SerializeField]
    private Text playerNameTagText = null;
    [SerializeField]
    private Text enemyNameTagText = null;
    [SerializeField]
    private MainGameLogicController logicController = null;

    #region DEBUGGING
    private GameFoundNotification GetDummyNotification()
    {
        return new GameFoundNotification
        {
            EnemyInfo = new UserInfo { Username = "ENEMY" },
            PlayersDeckSize = 30,
            PlayersHealth = 24,
            EnemiesDeckSize = 30,
            EnemiesHealth = 25
        };
    }

    private UserInfo GetDummyUserInfo()
    {
        return new UserInfo { Username = "YOU" };
    }
    #endregion

    private void Awake()
    {
        if (mapperContainer == null)
        {
            throw new ArgumentNullException(nameof(mapperContainer));
        }
        if (playerNameTagText == null)
        {
            throw new ArgumentNullException(nameof(playerNameTagText));
        }
        if (enemyNameTagText == null)
        {
            throw new ArgumentNullException(nameof(enemyNameTagText));
        }
        if (logicController == null)
        {
            throw new ArgumentNullException(nameof(logicController));
        }
    }

    private void Start()
    {
        //UserInfo userInfo = globalReference.UserInfoContainer.UserInfo;
        //GameFoundNotification gameNotification = globalReference.GameFoundNotification;

        UserInfo userInfo = GetDummyUserInfo();
        GameFoundNotification gameNotification = GetDummyNotification();

        PrepareNameTags(userInfo, gameNotification.EnemyInfo);

        PreparePlayerDatas(gameNotification);

        //NotifyReadyStatus();
    }

    private void PreparePlayerDatas(GameFoundNotification gameNotification)
    {
        logicController.playersDataController.DeckSize = gameNotification.PlayersDeckSize;
        logicController.playersDataController.Health = gameNotification.PlayersHealth;
        logicController.playersDataController.Mana = 0;
        logicController.playersDataController.HandSize = 0;

        logicController.enemiesDataController.DeckSize = gameNotification.EnemiesDeckSize;
        logicController.enemiesDataController.Health = gameNotification.EnemiesHealth;
        logicController.enemiesDataController.Mana = 0;
        logicController.enemiesDataController.HandSize = 0;
    }

    private void NotifyReadyStatus()
    {
        globalReference.GameClient.Send(new UserReadyNotification());
    }

    private void PrepareNameTags(UserInfo playerInfo, UserInfo enemyInfo)
    {
        playerNameTagText?.SetRegularText(playerInfo.Username);
        enemyNameTagText?.SetRegularText(enemyInfo.Username);
    }
}
