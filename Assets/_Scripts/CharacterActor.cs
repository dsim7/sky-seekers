using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CharacterActor : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    CharacterActorMover mover;
    [Space]
    [SerializeField]
    TargetingRaycaster actorSelector;
    
    public Character Character { get; set; }
    public CharacterActorMover Mover => mover;

    Animator animator;
    UnityAction currentAnimationHitEffect;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveToMelee(CharacterActor actor)
    {
        mover.MoveToMelee(actor);
    }

    public bool HasAnimation(string animationName)
    {
        return animator.parameters.Any(p => p.name == animationName);
    }

    public void PlayAnimation(string animationName)
    {
        animator.SetTrigger(animationName);
    }

    public void SetCurrentAnimationHitEffect(UnityAction effect)
    {
        currentAnimationHitEffect = effect;
    }

    // Animation callback
    public void DoAnimationHitEffect()
    {
        currentAnimationHitEffect.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (actorSelector != null)
        {
            actorSelector.InvokeCharacterSelect(this);
        }
    }
}
