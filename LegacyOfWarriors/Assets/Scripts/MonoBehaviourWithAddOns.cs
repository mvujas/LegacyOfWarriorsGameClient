using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Delegates;

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
}
