﻿using Remote.InGameObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardListLoader : MonoBehaviour
{
    [SerializeField]
    private string cardListFileName = "cardList.dat";

    private string basePath = null;

    private string cardListFilePath = null;

    public CardListLoader()
    {
        if(Application.isEditor)
        {
            basePath = "Assets/Resources/Cache";
        }
        else
        {
            basePath = "LegacyOfWarriors_Data/Resources";
        }
    }

    private void Awake()
    {
        cardListFilePath = $"{basePath}/{cardListFileName}";
    }

    public CardList LoadCardList()
    {
        try
        {
            CardList cardList = Utils.SeriabilityUtils.ReadObjectFromFile<CardList>(cardListFilePath);
            return cardList;
        }
        catch(Exception)
        {
            return null;
        }
    } 

    public bool SaveCardList(CardList cardList)
    {
        try
        {
            Utils.SeriabilityUtils.SaveObjectToFile(cardList, cardListFilePath);
#if UNITY_EDITOR
            if (Application.isEditor)
            {
                AssetDatabase.Refresh();
            }
#endif
            return true;
        }
        catch(Exception)
        {
            return false;
        }
    }
}
