﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectLevelConfig;
using System;
using ClientUtils;
using System.Net;
using Utils.Net;

[RequireComponent(typeof(ExecutionQueue))]
public class GlobalReferenceInitializer : MonoBehaviour
{
    #region EDITOR FIELD CLASSES
    [Serializable]
    public class NetworkSettings
    {
        public string host = EndPointConfig.HOST;
        public int port = EndPointConfig.PORT;
        public int bufferSize = 10;
    }
    #endregion

    #region EDITOR FIELDS
    [SerializeField]
    private NetworkSettings networkSettings = new NetworkSettings();

    private void OnValidate()
    {
        networkSettings.bufferSize = Math.Max(1, networkSettings.bufferSize);
    }
    #endregion

    private GlobalReference m_globalReference = GlobalReference.GetInstance();

    private void Awake()
    {
        InitializeGameClient();
        SetExecutionQueue();
    }

    private void SetExecutionQueue()
    {
        m_globalReference.ExecutionQueue = GetComponent<ExecutionQueue>();
    }

    private void InitializeGameClient()
    {
        IPEndPoint endPoint = NetUtils.CreateEndPoint(networkSettings.host, networkSettings.port);
        GameClientSpec spec = new GameClientSpec
        {
            endPoint = endPoint,
            bufferSize = networkSettings.bufferSize
        };
        m_globalReference.GameClient = new GameClient(spec);
    }
}
