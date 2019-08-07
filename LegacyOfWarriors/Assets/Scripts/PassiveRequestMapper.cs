using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Remote;
using Remote.Interface;

public abstract class PassiveRequestMapper : RemoteRequestMapper
{
    protected Dictionary<Type, RequestHandler> mapperDictionary = null;

    protected override Dictionary<Type, RequestHandler> GetMapperDictionary()
    {
        return mapperDictionary;
    }

    protected override IRemoteObject InvalidTypeRepsonse(IRemoteObject remoteObject)
    {
        Debug.Log("Invalid response: " + remoteObject.GetType());
        return null;
    }
}