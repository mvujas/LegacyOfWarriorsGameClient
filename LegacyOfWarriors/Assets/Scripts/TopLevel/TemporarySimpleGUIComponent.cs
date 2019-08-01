using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Remote;
using ClientUtils;

public abstract class TemporarySimpleGUIComponent : MonoBehaviourWithAddOns
{
    protected abstract RemoteRequestMapper GetRemoteRequestMapper();

    private GlobalReference m_globalReference = GlobalReference.GetInstance();
    private GameClient m_gameClient = null;

    public virtual void Show()
    {
        m_gameClient = m_globalReference.GameClient;
        transform.localScale = new Vector3(1, 1, 1);
        if (m_gameClient.IsActive())
        {
            m_gameClient.ChangeRequestMapper(GetRemoteRequestMapper());
        }
    }

    public virtual void Hide()
    {
        transform.localScale = new Vector3(0, 0, 0);
    }
}
