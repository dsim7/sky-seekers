using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ability
{
    CharacterAbilityHandler abilityHandler;
    public CharacterAbilityHandler AbilityHandler => abilityHandler;
    
    AbilityTemplate template;
    public AbilityTemplate Template => template;

    AbilityCooldown cooldown;
    public AbilityCooldown Cooldown => cooldown;

    AbilityInstance currentCast;

    BoolVariable canCast = new BoolVariable();
    public bool CanCast => canCast.Value;
    public void ListenToCanCast(UnityAction listener) { canCast.RegisterPostchangeEvent(listener); }
    public void UnlistenToCanCast(UnityAction listener) { canCast.UnregisterPostchangeEvent(listener); }

    public Ability(AbilityTemplate template, CharacterAbilityHandler abilityHandler)
    {
        this.template = template;
        this.abilityHandler = abilityHandler;
        
        cooldown = new AbilityCooldown(template.Cooldown);
        RegisterCanCastListeners();
    }

    public AbilityInstance GetNewInstance(Character caster)
    {
        currentCast = AbilityInstance.NewAbility(this, caster);
        return currentCast;
    }

    public void StartCasting(TargetingAI aiTargeting = null)
    {
        if (template.Targeting.IsAutoTarget)
        {
            AutoTarget(currentCast);
            currentCast.Execute();
        }
        else
        {
            if (currentCast.Caster.AIHandler.IsAI)
            {
                UseAITargets(currentCast, aiTargeting);
                currentCast.Execute();
            }
            else
            {
                ManualTarget(currentCast);
            }
        }
    }

    void AutoTarget(AbilityInstance abilityInstance)
    {
        template.Targeting.AutoTarget(abilityInstance);
    }

    void UseAITargets(AbilityInstance abilityInstance, TargetingAI aiTargeting = null)
    {
        if (aiTargeting == null)
        {
            return;
        }
        abilityInstance.Targets.AddRange(aiTargeting.GetAITargets(abilityInstance));
        if (!abilityInstance.CheckTargets())
        {
            return;
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
        abilityHandler.Character.ActionPointHandler.ListenToPoints(UpdateCanCast);
        cooldown.ListenToCD(UpdateCanCast);
    }

    void UpdateCanCast()
    {
        bool results = abilityHandler.Character.ActionPointHandler.HavePoints(template.ActionPointCost) &&
            cooldown.IsOffCD;
        if (results != canCast.Value)
        {
            canCast.Value = results;
        }
    }
}
