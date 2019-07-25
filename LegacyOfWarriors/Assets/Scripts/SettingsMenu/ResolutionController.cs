using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Dropdown))]
public class ResolutionController : MonoBehaviourWithAddOns
{
    private Resolution[] resolutions = null;
    private Dropdown dropdown = null;

    private void Awake()
    {
        resolutions = Screen.resolutions;
        PrepareValues();
    }

    private void PrepareValues()
    {
        List<string> resolutionStringList = resolutions.Select(res => res.ToString()).ToList<string>();
        dropdown.AddOptions(resolutionStringList);
    }
}
