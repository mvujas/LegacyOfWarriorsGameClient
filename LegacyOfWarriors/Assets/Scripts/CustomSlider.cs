using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utils.Delegates;

[RequireComponent(typeof(Slider))]
public class CustomSlider : MonoBehaviour
{
    #region EDITOR FIELDS
    [SerializeField]
    private float timeInSecondsToFillUp = 1;

    private void OnValidate()
    {
        timeInSecondsToFillUp = Mathf.Max(timeInSecondsToFillUp, .1f);
    }
    #endregion

    private float m_targetPercent;
    private float fillingSpeed;
    private Slider m_slider;
    public Runnable OnSliderFillUp { get; set; }

    public float Percent
    {
        get => m_targetPercent;
        set => m_targetPercent = Mathf.Clamp01(value);
    }

    private void Awake()
    {
        fillingSpeed = 1 / timeInSecondsToFillUp;
        m_slider = GetComponent<Slider>();
        m_targetPercent = m_slider.value;
    }

    private void FixedUpdate()
    {
        float difference = m_targetPercent - m_slider.value;
        if(!Utils.NumberUtils.FloatEquals(difference, 0f))
        {
            float newValue = m_slider.value + fillingSpeed * Time.fixedDeltaTime;
            if(difference > 0)
            {
                m_slider.value = Mathf.Min(m_targetPercent, newValue);
            }
            else
            {
                m_slider.value = Mathf.Max(m_targetPercent, newValue);
            }

            if(Utils.NumberUtils.FloatEquals(m_slider.value, 1))
            {
                OnSliderFillUp?.Invoke();
            }
        }
    }
}
