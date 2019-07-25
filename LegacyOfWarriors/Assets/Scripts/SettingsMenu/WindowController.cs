using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class WindowController : MonoBehaviourWithAddOns
{
    [SerializeField]
    private Dropdown resolutionDropdown = null;
    [SerializeField]
    private Dropdown screenModeDropdown = null;

    private Dictionary<FullScreenMode, string> modeToStringMapping = new Dictionary<FullScreenMode, string>
    {
        [FullScreenMode.ExclusiveFullScreen] = "FULL SCREEN",
        [FullScreenMode.FullScreenWindow] = "BORDERLESS",
        [FullScreenMode.Windowed] = "WINDOWED"
    };
    private Dictionary<string, FullScreenMode> stringToModeMapping = null;

    private Resolution[] resolutions = null;

    private void Awake()
    {
        stringToModeMapping = modeToStringMapping.ToDictionary(x => x.Value, x => x.Key);
        resolutions = Screen.resolutions;

        InitializeResolutionDropdown();
        InitializeScreenModeDropdown();
    }

    private void InitializeScreenModeDropdown()
    {
        if(Application.isEditor)
        {
            screenModeDropdown.AddOptions(new List<string> { "Nije podrzano u editoru" });
            return;
        }
        string initialOption = modeToStringMapping[Screen.fullScreenMode];
        List<string> options = stringToModeMapping.Select(x => x.Key).ToList<string>();
        int optionIndex = 0;
        for (int i = 0; i < options.Count; i++)
        {
            if (options[i] == initialOption)
            {
                optionIndex = i;
            }
        }
        screenModeDropdown.AddOptions(options);
        screenModeDropdown.value = optionIndex;
        screenModeDropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged();
        });
    }

    private void InitializeResolutionDropdown()
    {
        Resolution currentResolution = Screen.currentResolution;
        int currentResolutionId = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].Equals(currentResolution))
            {
                currentResolutionId = i;
            }
        }
        List<string> resolutionStringList = resolutions.Select(res => $"{res.width}x{res.height}").ToList<string>();
        resolutionDropdown.AddOptions(resolutionStringList);
        resolutionDropdown.value = currentResolutionId;
        resolutionDropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged();
        });
    }

    private Resolution GetSelectedResolution()
    {
        int optionId = resolutionDropdown.value;
        Resolution resolution = resolutions[optionId];
        return resolution;
    }

    private FullScreenMode GetSelectedFullScreenMode()
    {
        string selectedOption = screenModeDropdown.options[screenModeDropdown.value].text;
        FullScreenMode mode = stringToModeMapping[selectedOption];
        return mode;
    }

    private void DropdownValueChanged()
    {
        if(Application.isEditor)
        {
            return;
        }
        Resolution resolution = GetSelectedResolution();
        FullScreenMode fullScreenMode = GetSelectedFullScreenMode();
        Screen.SetResolution(resolution.width, resolution.height, fullScreenMode);
    }

}
