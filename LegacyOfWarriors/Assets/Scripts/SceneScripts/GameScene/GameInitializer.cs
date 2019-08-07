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

    #region DEBUGGING
    private GameFoundNotification GetDummyNotification()
    {
        return new GameFoundNotification
        {
            EnemyInfo = new UserInfo { Username = "ENEMY" }
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
    }

    private void Start()
    {
        //UserInfo userInfo = globalReference.UserInfoContainer.UserInfo;
        //GameFoundNotification gameNotification = globalReference.GameFoundNotification;

        UserInfo userInfo = GetDummyUserInfo();
        GameFoundNotification gameNotification = GetDummyNotification();

        PrepareNameTags(userInfo, gameNotification.EnemyInfo);

        NotifyReadyStatus();
    }

    private void NotifyReadyStatus()
    {
        //globalReference.GameClient.Send(null);
    }

    private void PrepareNameTags(UserInfo playerInfo, UserInfo enemyInfo)
    {
        playerNameTagText?.SetRegularText(playerInfo.Username);
        enemyNameTagText?.SetRegularText(enemyInfo.Username);
    }
}
