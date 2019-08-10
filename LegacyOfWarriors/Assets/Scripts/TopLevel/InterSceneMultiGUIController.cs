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
        public bool isActiveOnNonLoggedStart = false;
        public bool isActiveOnLoggedStart = false;
    }

    [SerializeField]
    private TemporaryGUIContainer[] guis = { };
    private TemporaryGUIContainer m_activeOneNonLogged = null;
    private TemporaryGUIContainer m_activeOneLogged = null;

    private Dictionary<string, TemporarySimpleGUIComponent> m_guiComponents = 
        new Dictionary<string, TemporarySimpleGUIComponent>();

    private TemporarySimpleGUIComponent m_currentGui = null;

    private void OnValidate()
    {
        /*var startingGuis = guis.Where(gui => gui != null && gui.isActiveOnNonLoggedStart).ToArray();
        switch (startingGuis.Length)
        {
            case 0:
                break;
            case 1:
                m_activeOneNonLogged = startingGuis[0];
                break;
            default:
                if(m_activeOneNonLogged == null)
                {
                    m_activeOneNonLogged = startingGuis[0];
                }
                foreach(var gui in startingGuis)
                {
                    if(gui != m_activeOneNonLogged)
                    {
                        gui.isActiveOnNonLoggedStart = false;
                    }
                }
                break;
        }*/

        AssureThatOnlyOneIsSelected(gui => gui.isActiveOnNonLoggedStart, (gui, val) => gui.isActiveOnNonLoggedStart = val, ref m_activeOneNonLogged);
        AssureThatOnlyOneIsSelected(gui => gui.isActiveOnLoggedStart, (gui, val) => gui.isActiveOnLoggedStart = val, ref m_activeOneLogged);
    }

    private void AssureThatOnlyOneIsSelected(Func<TemporaryGUIContainer, bool> getter, Action<TemporaryGUIContainer, bool> setter, 
        ref TemporaryGUIContainer selectedField)
    { 
        var startingGuis = guis.Where(gui => gui != null && getter.Invoke(gui)).ToArray();
        switch (startingGuis.Length)
        {
            case 0:
                break;
            case 1:
                selectedField = startingGuis[0];
                break;
            default:
                if (selectedField == null)
                {
                    selectedField = startingGuis[0];
                }
                foreach (var gui in startingGuis)
                {
                    if (gui != selectedField)
                    {
                        setter.Invoke(gui, false);
                    }
                }
                break;
        }
    }

    private void SelectActive(Func<TemporaryGUIContainer, bool> getter, ref TemporaryGUIContainer selectedField)
    {
        var activeGuis = guis.Where(gui => gui != null && getter.Invoke(gui)).ToArray();
        selectedField = activeGuis.Length == 1 ? activeGuis[0] : null;

        if (selectedField != null && (selectedField.guiSwitcher == null || selectedField.name.Length == 0))
        {
            selectedField = null;
        }
    }

    private void RemoveNullContainers()
    {
        guis = guis.Where(gui => gui != null && gui.guiSwitcher != null && gui.name.Length > 0).ToArray();


        /*var activeGuisNonLogged = guis.Where(gui => gui != null && gui.isActiveOnNonLoggedStart).ToArray();
        m_activeOneNonLogged = activeGuisNonLogged.Length == 1 ? activeGuisNonLogged[0] : null;
        if (m_activeOneNonLogged != null && (m_activeOneNonLogged.guiSwitcher == null || m_activeOneNonLogged.name.Length == 0))
        {
            m_activeOneNonLogged = null;
        }*/

        SelectActive(gui => gui.isActiveOnLoggedStart, ref m_activeOneLogged);
        SelectActive(gui => gui.isActiveOnNonLoggedStart, ref m_activeOneNonLogged);
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

        if (globalReference.UserInfoContainer == null || globalReference.UserInfoContainer.UserInfo == null)
        {
            if (m_activeOneNonLogged != null)
            {
                Show(m_activeOneNonLogged.name);
            }
        }
        else if(globalReference.UserInfoContainer.UserInfo != null)
        {
            if(m_activeOneLogged != null)
            {
                Show(m_activeOneLogged.name);
            }
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
