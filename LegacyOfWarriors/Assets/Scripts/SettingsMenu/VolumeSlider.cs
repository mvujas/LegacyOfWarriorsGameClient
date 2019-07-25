using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviourWithAddOns
{
    [SerializeField]
    private Slider slider = null;

    private void Awake()
    {
        if(slider != null)
        {
            slider.value = AudioListener.volume;
        }
    }

    public void SetVolumeToValue()
    {
        AudioListener.volume = slider.value;
    }
}
