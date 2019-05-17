using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterActorSelectEvent : MonoBehaviour
{
    [SerializeField]
    Camera raycastCamera;

    CharacterSelectedEvent selectionEvent;
    
    void Awake()
    {
        selectionEvent = new CharacterSelectedEvent();
    }

    public void RegisterListener(UnityAction<CharacterActor> listener)
    {
        selectionEvent.AddListener(listener);
    }

    public void InvokeCharacterSelect(CharacterActor actor)
    {
        selectionEvent.Invoke(actor);
    }
}

public class CharacterSelectedEvent : UnityEvent<CharacterActor> { }