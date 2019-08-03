using ClientUtils;
using Remote.InGameObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReference
{
    #region SINGLETON SETUP
    private GlobalReference() { }

    private static GlobalReference instance = new GlobalReference();

    public static GlobalReference GetInstance()
    {
        return instance;
    }
    #endregion

    public GameClient GameClient { get; set; }
    public ExecutionQueue ExecutionQueue { get; set; }
    public SceneController SceneController { get; set; }
    public CardList CardList { get; set; }
    public UserInfoContainer UserInfoContainer { get; private set; } = new UserInfoContainer();
}
