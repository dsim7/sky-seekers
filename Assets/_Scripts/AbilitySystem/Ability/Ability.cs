using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ability
{
    public CharacterAbilityHandler AbilityHandler { get; }
    public AbilityTemplate Template { get; }
    public AbilityCooldown Cooldown { get; }

    BoolVariable canCast = new BoolVariable();
    public bool CanCast => canCast.Value;
    public void ListenToCanCast(UnityAction listener) { canCast.RegisterPostchangeEvent(listener); }
    public void UnlistenToCanCast(UnityAction listener) { canCast.UnregisterPostchangeEvent(listener); }

    AbilityInstance currentCast;

    public Ability(AbilityTemplate template, CharacterAbilityHandler abilityHandler)
    {
        Template = template;
        AbilityHandler = abilityHandler;
        
        Cooldown = new AbilityCooldown(template.Cooldown);
        RegisterCanCastListeners();
    }

    public AbilityInstance GetNewInstance(Character caster)
    {
        currentCast = AbilityInstance.NewAbility(this, caster);
        return currentCast;
    }

    public bool StartCasting(TargetingAI aiTargeting = null)
    {
        if (Template.Targeting.IsAutoTarget)
        {
            AutoTarget(currentCast);
            if (!currentCast.CheckTargets())
            {
                currentCast.Cancel();
                return false;
            }
            currentCast.Execute();
        }
        else
        {
            if (currentCast.Caster.AIHandler.IsAI)
            {
                UseAITargets(currentCast, aiTargeting);
                if (!currentCast.CheckTargets())
                {
                    currentCast.Cancel();
                    return false;
                }
                currentCast.Execute();
            }
            else
            {
                ManualTarget(currentCast);
            }
        }
        return true;
    }

    void AutoTarget(AbilityInstance abilityInstance)
    {
        Template.Targeting.AutoTarget(abilityInstance);
    }

    void UseAITargets(AbilityInstance abilityInstance, TargetingAI aiTargeting = null)
    {
        if (aiTargeting == null)
        {
            return;
        }
        List<Character> targets = aiTargeting.GetAITargets(abilityInstance);
        if (targets != null)
        {
            abilityInstance.Targets.AddRange(targets);
        }
    }

    void ManualTarget(AbilityInstance abilityInstance)
    {
        TargetingController targetingController = abilityInstance.Caster.Team.TargetingController;
        
        targetingController.ChooseNewTargets(Template.Targeting.NumTargetsRequired,
            character => Template.Targeting.SelectTargetCheck(character, abilityInstance),
            targets =>
            {
                abilityInstance.Targets.AddRange(targets);
                abilityInstance.Execute();
            },
            abilityInstance.Cancel);
    }

    void RegisterCanCastListeners()
    {
        AbilityHandler.Character.ActionPointHandler.ListenToPoints(UpdateCanCast);
        Cooldown.ListenToCD(UpdateCanCast);
        if (Template.MustBeAbleToReposition)
        {
            AbilityHandler.Character.PositionHandler.ListenToCanReposition(UpdateCanCast);
        }
    }

    void UpdateCanCast()
    {
        bool results = Cooldown.IsOffCD && 
            AbilityHandler.Character.ActionPointHandler.HavePoints(Template.ActionPointCost) &&
            (!Template.MustBeAbleToReposition || AbilityHandler.Character.PositionHandler.CanReposition);
        if (results != canCast.Value)
        {
            canCast.Value = results;
        }
    }
}
