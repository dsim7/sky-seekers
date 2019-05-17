using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ObservedVariable<T>
{
    [SerializeField]
    T _value;
    public T Value { get { return _value; } set { _prechangeEvent.Invoke(); _value = value; _postchangeEvent.Invoke(); } }

    UnityEvent _prechangeEvent = new UnityEvent();
    UnityEvent _postchangeEvent = new UnityEvent();

    public void RegisterPrechangeEvent(UnityAction action)
    {
        _prechangeEvent.AddListener(action);
    }

    public void UnregisterPrechangeEvent(UnityAction action)
    {
        _prechangeEvent.RemoveListener(action);
    }

    public void RegisterPostchangeEvent(UnityAction action)
    {
        _postchangeEvent.AddListener(action);
    }

    public void UnregisterPostchangeEvent(UnityAction action)
    {
        _postchangeEvent.RemoveListener(action);
    }

    public void Updated()
    {
        _postchangeEvent.Invoke();
    }
}
