

using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class AbilityActionInstance : IProlongedAction
{
    AbilityAction template;
    public AbilityAction Template => template;

    Character caster;
    public Character Caster => caster;

    List<Character> targets;
    public List<Character> Targets => targets;

    OneTimeEvent onComplete = new OneTimeEvent();
    OneTimeEvent onPostComplete = new OneTimeEvent();
    
    public AbilityActionInstance(AbilityAction actionTemplate, Character caster, List<Character> targets)
    {
        template = actionTemplate;
        this.caster = caster;
        this.targets = targets;
    }

    public OneTimeEvent GetActionCompleteEvent()
    {
        return onComplete;
    }

    public OneTimeEvent GetActionPostCompleteEvent()
    {
        return onPostComplete;
    }

    public float GetPostActionDelay()
    {
        return template.DelayToNextAction;
    }

    public void StartAction()
    {
        caster.Actor.DoAction(this);
    }
}
