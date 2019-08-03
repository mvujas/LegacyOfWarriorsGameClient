using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class Logging : MonoBehaviourWithAddOns
{
    [SerializeField]
    private const string logFileName = "gameLog.txt";
    private string fullPath;
    
    private void Awake()
    {
        string basePath;
        if (Application.isEditor)
        {
            basePath = "Assets/Resources";
        }
        else
        {
            basePath = "LegacyOfWarriors_Data/Resources";
        }

        fullPath = $"{basePath}/{logFileName}";
    }

    public void WriteToLog(string input)
    {
        if (!File.Exists(fullPath))
        {
            using (StreamWriter sw = File.CreateText(fullPath))
            {
                sw.WriteLine(input);
            }

        }
        else
        {
            using (StreamWriter sw = File.AppendText(fullPath))
            {
                sw.WriteLine(input);
            }
        }
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogCallback;
    }

    void LogCallback(string condition, string stackTrace, LogType type)
    {
        WriteToLog($"===== LOGTYPE: {type} \n CONDITION: {condition}\n STACK TRACE: {stackTrace}");
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogCallback;
    }
}
