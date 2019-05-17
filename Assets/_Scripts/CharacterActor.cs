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
    SpriteRenderer sprite;
    [Space]
    [SerializeField]
    CharacterActorSelectEvent actorSelector;

    Character owner;
    public Character Character { get { return owner; } set { owner = value; owner.ListenToAvailable(UpdateSprite); } }

    Animator animator;
    UnityAction onCurAnimationHit;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void UpdateSprite()
    {
        sprite.color = Character.Available ? Color.red : Color.blue;
    }

    public bool HasAnimation(string animationName)
    {
        return animator.parameters.Any(p => p.name == animationName);
    }

    public void PlayAnimation(string animationName)
    {
        animator.SetTrigger(animationName);
    }

    // Animation callback
    public void DoAnimationHitEffect()
    {
        onCurAnimationHit.Invoke();
    }

    public void DoAction(AbilityActionInstance actInst)
    {
        string animName = actInst.Template.AnimationName;

        if (actInst.Template.IsMelee)
        {
            Character.Mover.MoveToMelee(actInst.Targets[actInst.Template.IndexOfPrimaryTarget].Actor);
        }

        if (animName == default(string) || !HasAnimation(animName))
        {
            actInst.Template.DoAction(actInst.Caster, actInst.Targets);
            actInst.GetActionCompleteEvent().Invoke();
        }
        else
        {
            PlayAnimation(animName);

            onCurAnimationHit = () =>
            {
                actInst.Template.DoAction(actInst.Caster, actInst.Targets);
                actInst.GetActionCompleteEvent().Invoke();
            };
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (actorSelector != null)
        {
            actorSelector.InvokeCharacterSelect(this);
        }
    }
}
