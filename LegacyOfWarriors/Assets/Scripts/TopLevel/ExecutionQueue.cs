using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using Utils.Delegates;

public class ExecutionQueue : MonoBehaviour
{
    #region EDITOR FIELDS
    [Serializable]
    public enum ExecutionMode
    {
        onUpdate,
        onFixedUpdate,
        supress
    }

    [SerializeField]
    private ExecutionMode executionMode = ExecutionMode.onUpdate;
    #endregion

    private ConcurrentQueue<Runnable> m_taskQueue = new ConcurrentQueue<Runnable>();

    public void Add(Runnable task)
    {
        m_taskQueue.Enqueue(task);
    }

    private void ExecuteTasks()
    {
        Runnable task;
        while(m_taskQueue.TryDequeue(out task))
        {
            task?.Invoke();
        }
    }

    private void Update()
    {
        if(executionMode == ExecutionMode.onUpdate)
        {
            ExecuteTasks();
        }
    }

    private void FixedUpdate()
    {
        if(executionMode == ExecutionMode.onFixedUpdate)
        {
            ExecuteTasks();
        }
    }
}
