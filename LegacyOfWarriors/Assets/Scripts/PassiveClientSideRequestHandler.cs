﻿using System;
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
        Debug.Log("Usao!");
        try
        {
            Handle(request as T);
        }
        catch(Exception e)
        {
            executionQueue.Add(() => Debug.Log("Izuzetak: " + e.Message));
        }
        return null;
    }

    public abstract void Handle(T obj);
}
