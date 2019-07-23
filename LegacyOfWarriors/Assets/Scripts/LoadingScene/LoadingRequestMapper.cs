using System;
using System.Collections;
using System.Collections.Generic;
using Remote.Interface;
using UnityEngine;
using Utils.Remote;
using Remote.Implementation;
using Utils.Net;
using Utils.Delegates;
using Remote.InGameObjects;

class CardListResponseHandle : PassiveClientSideRequestHandler<CardListResponse>
{
    public Runnable OnUpToDate { get; set; }
    public Action<CardList> OnNotUpToDate { get; set; }

    public override void Handle(CardListResponse obj)
    {
        if(obj.UpToDate)
        {
            OnUpToDate?.Invoke();
        }
        else
        {
            OnNotUpToDate?.Invoke(obj.CardList);
        }
        Debug.Log("Ovde!");
    }
}


public class LoadingRequestMapper : RemoteRequestMapper
{
    private Dictionary<Type, RequestHandler> m_mapper;

    public LoadingRequestMapper(Runnable onUpToDate, Action<CardList> onNotUpToDate)
    {
        m_mapper = new Dictionary<Type, RequestHandler>
        {
            [typeof(CardListResponse)] = new CardListResponseHandle
            {
                OnUpToDate = onUpToDate,
                OnNotUpToDate = onNotUpToDate
            }
        };
    }

    protected override Dictionary<Type, RequestHandler> GetMapperDictionary()
    {
        return m_mapper;
    }

    protected override IRemoteObject InvalidTypeRepsonse()
    {
        return null;
    }
}
