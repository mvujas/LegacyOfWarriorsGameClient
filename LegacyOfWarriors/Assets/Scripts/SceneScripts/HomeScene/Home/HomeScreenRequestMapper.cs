using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Remote;
using Remote.Interface;
using Remote.Implementation;

public class HomeScreenRequestMapper : PassiveRequestMapper
{
    public HomeScreenRequestMapper(
        Action<QueueEntryResponse> onQueueEntryResponse,
        Action<QueueExitResponse> onQueueExitReponse)
    {
        mapperDictionary = new Dictionary<Type, RequestHandler>
        {
            [typeof(QueueEntryResponse)] = new QueueEntryResponseHandler { Handler = onQueueEntryResponse },
            [typeof(QueueExitResponse)] = new QueueExitResponseHandler { Handler = onQueueExitReponse }
        };
    }
}
