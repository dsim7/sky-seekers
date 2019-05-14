using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
    AbilityTemplate template;
    AbilityCooldown cooldown;
    int actionPointCost;
    bool beingCast;

    public AbilityTemplate Template => template;
    public AbilityCooldown Cooldown => cooldown;
    public int ActionPointCost => actionPointCost;
    public bool BeingCast => beingCast;

    public Ability(AbilityTemplate template)
    {
        this.template = template;
        actionPointCost = template.ActionPointCost;

        cooldown = new AbilityCooldown(template.Cooldown);
    }

    public AbilityInstance Cast(Character caster, List<Character> AItargets = null)
    {
        if (!cooldown.IsOffCD)
        {
            return null;
        }
        
        beingCast = true;
        AbilityInstance abilityInstance = AbilityInstance.NewAbility(this, caster);

        if (template.Targeting.IsAutoTarget)
        {
            AutoTarget(abilityInstance);
            Execute(abilityInstance);
        }
        else
        {
            if (caster.TeamHandler.Team.AI != null)
            {
                AITarget(abilityInstance, AItargets);
                Execute(abilityInstance);
            }
            else
            {
                ManualTarget(abilityInstance);
            }
        }
        return abilityInstance;
    }

    void AutoTarget(AbilityInstance abilityInstance)
    {
        template.Targeting.AutoTarget(abilityInstance);
    }

    void AITarget(AbilityInstance abilityInstance, List<Character> AItargets)
    {
        if (AItargets == null)
        {
            return;
        }
        abilityInstance.Targets.AddRange(AItargets);
        if (!abilityInstance.CheckTargets())
        {
            return;
        }
    }

    void ManualTarget(AbilityInstance abilityInstance)
    {
        TargetingController targetingController = abilityInstance.Caster.TeamHandler.Team.TargetingController;

        targetingController.ChooseNewTargets(Template.Targeting.NumTargetsRequired,
            character => Template.Targeting.SelectTargetCheck(character, abilityInstance),
            targets =>
            {
                abilityInstance.Targets.AddRange(targets);
                Execute(abilityInstance);
            },
            abilityInstance.Cancel);
    }

    void Execute(AbilityInstance abilityInstance)
    {
        abilityInstance.RegisterOnCompleteListener(() => beingCast = false);
        abilityInstance.Execute();
    }
}
