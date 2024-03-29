﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Remote.Implementation;

public class UserInfoContainer
{
    private UserInfo m_userInfo = null;
    public UserInfo UserInfo
    {
        get
        {
            if(m_userInfo == null || m_userInfo.Username == null)
            {
                return null;
            }
            return m_userInfo;
        }
        set
        {
            m_userInfo = value;
        }
    }
}
