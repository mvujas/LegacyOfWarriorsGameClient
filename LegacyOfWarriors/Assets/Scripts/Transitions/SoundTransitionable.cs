using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.Delegates;

public class SoundTransitionable : MonoBehaviourWithAddOns
{
    [SerializeField]
    private AudioSource audioSource = null;
    [SerializeField]
    private float endVolume = 1;
    [SerializeField]
    private float m_animationDuration = 0.1f;
    [SerializeField]
    private bool runOnStart = false;
    public float AnimationDuration
    {
        get => m_animationDuration;
        set => m_animationDuration = Mathf.Max(0.01f, m_animationDuration);
    }

    private void OnValidate()
    {
        AnimationDuration = m_animationDuration;
        endVolume = Mathf.Clamp01(endVolume);
    }

    private static readonly TransitionFunction<float> linear = (t, b, c, d) =>
    {
        return c * t / d + b;
    };

    private float initialVolume;
    private float fullVolumeDifference;

    protected virtual void Awake()
    {
        initialVolume = audioSource.volume;
        fullVolumeDifference = Mathf.Abs(endVolume - initialVolume);
    }

    private void Start()
    {
        if (runOnStart)
        {
            GoToEndVolume();
        }
    }

    private void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    private void AnimateChangeToTargetVolume(float targetVolume, Supplier<bool> exitPredicate)
    {
        float pathRatio = Mathf.Abs(audioSource.volume - targetVolume) / fullVolumeDifference;
        if (pathRatio < 0.01f)
        {
            return;
        }
        float time = pathRatio * AnimationDuration;
        float change = targetVolume - audioSource.volume;
        PlayTransition<float>(linear, time, audioSource.volume, change, SetVolume, exitPredicate);
    }

    public void GoToEndVolume(Supplier<bool> exitPredicate = null)
    {
        AnimateChangeToTargetVolume(endVolume, exitPredicate);
    }

    public void GoToInitialVolume(Supplier<bool> exitPredicate = null)
    {
        AnimateChangeToTargetVolume(endVolume, exitPredicate);
    }
}
