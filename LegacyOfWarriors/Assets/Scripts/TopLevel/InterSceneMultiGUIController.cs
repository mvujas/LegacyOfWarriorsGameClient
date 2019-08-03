using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class InterSceneMultiGUIController : MonoBehaviourWithAddOns
{
    [Serializable]
    public class TemporaryGUIContainer
    {
        public string name;
        public TemporarySimpleGUIComponent guiSwitcher;
        public bool isActiveOnStart = false;
    }

    [SerializeField]
    private TemporaryGUIContainer[] guis = { };
    private TemporaryGUIContainer m_activeOne = null;

    private Dictionary<string, TemporarySimpleGUIComponent> m_guiComponents = 
        new Dictionary<string, TemporarySimpleGUIComponent>();

    private TemporarySimpleGUIComponent m_currentGui = null;

    private void OnValidate()
    {
        var startingGuis = guis.Where(gui => gui != null && gui.isActiveOnStart).ToArray();
        switch (startingGuis.Length)
        {
            case 0:
                break;
            case 1:
                m_activeOne = startingGuis[0];
                break;
            default:
                if(m_activeOne == null)
                {
                    m_activeOne = startingGuis[0];
                }
                foreach(var gui in startingGuis)
                {
                    if(gui != m_activeOne)
                    {
                        gui.isActiveOnStart = false;
                    }
                }
                break;
        }
    }

    private void RemoveNullContainers()
    {
        guis = guis.Where(gui => gui != null && gui.guiSwitcher != null && gui.name.Length > 0).ToArray();
        var activeGuis = guis.Where(gui => gui != null && gui.isActiveOnStart).ToArray();
        m_activeOne = activeGuis.Length == 1 ? activeGuis[0] : null;
        if (m_activeOne != null && (m_activeOne.guiSwitcher == null || m_activeOne.name.Length == 0))
        {
            m_activeOne = null;
        }
    }

    private void PrepareDictionary()
    {
        foreach(var gui in guis)
        {
            if(!m_guiComponents.ContainsKey(gui.name))
            {
                m_guiComponents.Add(gui.name, gui.guiSwitcher);
            }
        }
    }

    private void Awake()
    {
        RemoveNullContainers();
        PrepareDictionary();
    }

    private void Start()
    {
        foreach(var guiPair in m_guiComponents)
        {
            guiPair.Value.Hide();
        }
        if (m_activeOne != null)
        {
            Show(m_activeOne.name);
        }
    }

    public void Show(string guiName)
    {
        if(!m_guiComponents.TryGetValue(guiName, out TemporarySimpleGUIComponent gui))
        {
            throw new InvalidOperationException("No such GUI");
        }
        if(m_currentGui != null)
        {
            m_currentGui.Hide();
        }
        m_currentGui = gui;
        gui.Show();
    }

}
