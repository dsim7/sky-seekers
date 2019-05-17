
using UnityEngine;
using UnityEngine.Events;

public class ObservedMonoBehaviour : MonoBehaviour
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

