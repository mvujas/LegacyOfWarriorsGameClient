using System;
using System.Collections;
using System.Collections.Generic;
using Remote.Interface;
using UnityEngine;
using Utils.Net;
using Utils.Remote;

public abstract class PassiveClientSideRequestHandler<T> : RequestHandler where T : class, IRemoteObject
{
    protected ExecutionQueue executionQueue = GlobalReference.GetInstance().ExecutionQueue;

    public IRemoteObject Handle(AsyncUserToken token, IRemoteObject request)
    {
#if UNITY_EDITOR
        try
        {
#endif
            Handle(request as T);
#if UNITY_EDITOR
        }
        catch (Exception e)
        {
            executionQueue.Add(() => Debug.Log("Izuzetak: " + e.Message));
        }
#endif
        return null;
    }

    public abstract void Handle(T obj);
}
