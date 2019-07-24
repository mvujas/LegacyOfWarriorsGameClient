using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Delegates;

public delegate T TransitionFunction<T>(float time, T start, T change, float duration);

public class MonoBehaviourWithAddOns : MonoBehaviour
{
    public static void RunInMainThread(Runnable function)
    {
        GlobalReference.GetInstance().ExecutionQueue.Add(function);
    }

    private static IEnumerator DelayedExecution(Runnable function, float delay)
    {
        yield return new WaitForSeconds(delay);
        function?.Invoke();
    }

    public void ExecuteAfterDelay(Runnable function, float delay)
    {
        StartCoroutine(DelayedExecution(function, delay));
    }

    public void PlayTransition<T>(TransitionFunction<T> transition, float time,
        T startValue, T targetValue, Action<T> ApplyTransition, Supplier<bool> exitPredicate = null)
    {
        StartCoroutine(PlayTransitionCoroutine<T>(transition, time,
                startValue, targetValue, ApplyTransition, exitPredicate));
    }

    private IEnumerator PlayTransitionCoroutine<T>(TransitionFunction<T> transition, float time,
        T startValue, T targetValue, Action<T> ApplyTransition, Supplier<bool> exitPredicate)
    {
        if (ApplyTransition == null)
        {
            yield break;
        }
        for (float value = 0; value <= time; value += Time.fixedDeltaTime)
        {
            if(exitPredicate != null && exitPredicate())
            {
                yield break;
            }
            var intermediateValue = transition(value, startValue, targetValue, time);
            ApplyTransition(intermediateValue);
            yield return new WaitForFixedUpdate();
        }
        ApplyTransition(transition(time, startValue, targetValue, time));
    }
}
