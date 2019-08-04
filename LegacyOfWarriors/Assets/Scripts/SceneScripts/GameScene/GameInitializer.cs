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
        UserInfo userInfo = GetDummyUserInfo();
        GameFoundNotification gameNotification = GetDummyNotification();

        PrepareNameTags(userInfo, gameNotification.EnemyInfo);
    }

    private void PrepareNameTags(UserInfo playerInfo, UserInfo enemyInfo)
    {
        playerNameTagText?.SetRegularText(playerInfo.Username);
        enemyNameTagText?.SetRegularText(enemyInfo.Username);
    }
}
