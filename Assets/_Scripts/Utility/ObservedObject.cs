using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObservedObject
{
    UnityEvent OnChange = new UnityEvent();

    public void RegisterListener(UnityAction action)
    {
        OnChange.AddListener(action);
    }

    public void UnregisterListener(UnityAction action)
    {
        OnChange.RemoveListener(action);
    }

    public void Changed()
    {
        OnChange.Invoke();
    }
}
