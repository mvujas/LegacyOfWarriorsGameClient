using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Remote;
using Remote.Interface;

public class MutablePassiveRequestMapper : PassiveRequestMapper
{
    public MutablePassiveRequestMapper()
    {
        mapperDictionary = new Dictionary<Type, RequestHandler>();
    }

    public void AddHandlerAction<T>(Action<T> handlerAction) where T : class, IRemoteObject
    {
        if(handlerAction == null)
        {
            throw new ArgumentNullException(nameof(handlerAction));
        }
        Type type = typeof(T);
        if(mapperDictionary.ContainsKey(type))
        {
            throw new InvalidOperationException("Handler for given type already exists");
        }
        mapperDictionary.Add(type, new PostponedPassiveClientRequestHandler<T>(handlerAction));
    }
}
