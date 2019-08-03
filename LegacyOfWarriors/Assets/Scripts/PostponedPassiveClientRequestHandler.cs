using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Remote.Interface;

public class PostponedPassiveClientRequestHandler<T> : PassiveClientSideRequestHandler<T> where T : class, IRemoteObject
{
    public Action<T> Handler { get; set; } = null;

    public PostponedPassiveClientRequestHandler(Action<T> handler)
    {
        Handler = handler;
    }

    public override void Handle(T obj)
    {
        if(Handler != null)
        {
            GlobalReference.GetInstance().ExecutionQueue.Add(() => Handler(obj));
        }
    }
}
