using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Remote;
using Remote.Interface;
using Utils.Delegates;
using Remote.Implementation;

public class LoginResponseHadler : PassiveClientSideRequestHandler<LoginResponse>
{
    public Action<UserInfo> OnLoginSucessful { get; set; }
    public Action<string> OnLoginFailed { get; set; }

    public override void Handle(LoginResponse obj)
    {
        if(obj.Successfulness)
        {
            OnLoginSucessful?.Invoke(obj.UserInfo);
        }
        else
        {
            OnLoginFailed?.Invoke(obj.Message);
        }
    }
}

public class LoginRequestMapper : RemoteRequestMapper
{
    private Dictionary<Type, RequestHandler> m_mapper;

    public LoginRequestMapper(Action<UserInfo> onLoginSuccessful, Action<string> onLoginFailed)
    {
        m_mapper = new Dictionary<Type, RequestHandler>
        {
            [typeof(LoginResponse)] = new LoginResponseHadler
            {
                OnLoginFailed = onLoginFailed,
                OnLoginSucessful = onLoginSuccessful
            }
        };
    }

    protected override Dictionary<Type, RequestHandler> GetMapperDictionary()
    {
        return m_mapper;
    }

    protected override IRemoteObject InvalidTypeRepsonse(IRemoteObject remoteObject)
    {
        return null;
    }
}
