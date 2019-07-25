using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class FullScreenModeController : MonoBehaviourWithAddOns
{
    private Dropdown dropdown = null;
    private Dictionary<FullScreenMode, string> modeToStringMapping = new Dictionary<FullScreenMode, string> {
        [FullScreenMode.ExclusiveFullScreen] = "FULL SCREEN",
        [FullScreenMode.FullScreenWindow] = "BORDERLESS",
        [FullScreenMode.Windowed] = "WINDOWED"
    };
    private Dictionary<string, FullScreenMode> stringToModeMapping = null;

    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
        if (!Application.isEditor)
        {
            stringToModeMapping = modeToStringMapping.ToDictionary(x => x.Value, x => x.Key);
            
            InitializeDropdown();
        }
        else
        {
            dropdown.AddOptions(new List<string>{ "Not avaliable in editor"});
        }
    }

    private void InitializeDropdown()
    {
        Debug.Log(Screen.fullScreenMode);
        string initialOption = modeToStringMapping[Screen.fullScreenMode];
        List<string> options = stringToModeMapping.Select(x => x.Key).ToList<string>();
        int optionIndex = 0;
        for(int i = 0; i < options.Count; i++)
        {
            if(options[i] == initialOption)
            {
                optionIndex = i;
            }
        }
        dropdown.AddOptions(options);
        dropdown.value = optionIndex;
        dropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged();
        });
    }

    private void DropdownValueChanged()
    {
        string selectedOption = dropdown.options[dropdown.value].text;
        FullScreenMode mode = stringToModeMapping[selectedOption];
        SetFullScreenMode(mode);
    }

    public void SetFullScreenMode(FullScreenMode mode)
    {
        Screen.fullScreenMode = mode;
    }

    public void HandleValueChange()
    {

    }
}
