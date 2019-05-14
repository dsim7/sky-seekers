using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TargetingRaycaster : MonoBehaviour
{
    [SerializeField]
    Camera raycastCamera;

    CharacterSelectedEvent selectionEvent;
    
    void Awake()
    {
        selectionEvent = new CharacterSelectedEvent();
    }

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
    //    {
    //        Debug.Log("tried targeting");
    //        CharacterActor target = HelperMethods.Raycast<CharacterActor>(raycastCamera);
    //        if (target != null)
    //        {
    //            SelectionEvent.Invoke(target);
    //        }
    //    }
    //}

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