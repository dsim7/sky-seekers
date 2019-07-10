using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class AbilityActionInstance : IProlongedAction
{
    public AbilityAction Template { get; }
    public Character Caster { get; }
    public List<Character> Targets { get; }

    OneTimeEvent onComplete = new OneTimeEvent();
    OneTimeEvent onPostComplete = new OneTimeEvent();
    
    public AbilityActionInstance(AbilityAction actionTemplate, Character caster, List<Character> targets)
    {
        Template = actionTemplate;
        Caster = caster;
        Targets = targets;
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
        return Template.DelayToNextAction;
    }

    public void StartAction()
    {
        Caster.Actor.DoAction(this);
    }
}
