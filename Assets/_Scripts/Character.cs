using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Character
{
    //public CharacterTemplate Template { get; private set; }
    //public CharacterActor Actor { get; private set; }
    //public CharacterTeamHandler TeamHandler { get; private set; }
    //public CharacterAbilityHandler AbilityHandler { get; private set; }
    //public CharacterEventHandler EventHandler { get; private set; }
    //public CharacterStatusHandler StatusHandler { get; private set; }
    //public CharacterHealthHandler HealthHandler { get; private set; }
    //public CharacterActionHandler ActionHandler { get; private set; }

    CharacterTemplate template;
    public CharacterTemplate Template => template;

    CharacterActor actor;
    public CharacterActor Actor => actor;

    CharacterTeamHandler teamHandler;
    public CharacterTeamHandler TeamHandler => teamHandler;

    CharacterAbilityHandler abilityHandler;

    CharacterEventHandler eventHandler;
    public CharacterEventHandler EventHandler => eventHandler;

    CharacterStatusHandler statusHandler;
    public CharacterStatusHandler StatusHandler => statusHandler;

    CharacterHealthHandler healthHandler;
    public CharacterHealthHandler HealthHandler => healthHandler;

    CharacterActionHandler actionHandler;
    public CharacterActionHandler ActionHandler => actionHandler;

    float power, critChance, critMultiplier, defense;

    bool busy;
    public bool Busy => busy;
    bool active;
    public bool Active => active;

    public Character(CharacterTemplate template, CharacterActor actor, Team team, TeamPosition position)
    {
        this.template = template;
        this.actor = actor;
        actor.Character = this;

        teamHandler = new CharacterTeamHandler(this, team, position);
        abilityHandler = new CharacterAbilityHandler(this);
        eventHandler = new CharacterEventHandler(this);
        statusHandler = new CharacterStatusHandler(this);
        healthHandler = new CharacterHealthHandler(this);
        actionHandler = new CharacterActionHandler(this);
    }

    public bool AwaitingCommand()
    {
        return !busy && active;
    }

    public void StartTurn()
    {
        actionHandler.ResetActionPoints();
        statusHandler.PerTurnStatuses();
        abilityHandler.TickCooldowns();
        busy = false;
        active = true;
    }

    public void FinishTurn()
    {
        statusHandler.RemoveExpiredStatuses();
        active = false;
    }

    public Ability GetAbility(AbilityType type)
    {
        return abilityHandler.GetAbility(type);
    }

    public AbilityInstance CastAbility(AbilityType abilityType)
    {
        Ability ability = abilityHandler.GetAbility(abilityType);
        if (AwaitingCommand())
        {
            if (actionHandler.HavePoints(ability.ActionPointCost))
            {
                busy = true;

                AbilityInstance abInst = abilityHandler.CastAbility(ability);
                if (abInst != null)
                {
                    abInst.RegisterOnStartListener(() =>
                    {
                        actionHandler.UseActionPoints(ability.ActionPointCost);
                        abInst.Caster.eventHandler.CastAbility.Invoke(abInst);
                        foreach (Character target in abInst.Targets)
                        {
                            target.eventHandler.TargetedByAbility.Invoke(abInst);
                        }
                    });
                    abInst.RegisterOnCompleteListener(() =>
                    {
                        busy = false;
                    });
                    abInst.RegisterOnCancelListener(() => busy = false);
                    return abInst;
                }
            }
            else
            {
                Debug.Log("Not enough action points");
            }
        }
        return null;    
    }

    public void DealDamage(DamageInstance dmgInst)
    {
        Character target = dmgInst.Target;

        dmgInst.Amount *= Mathf.Clamp(1 + ((abilityHandler.Power - 10) * 0.075f), 0, Mathf.Infinity);

        if (dmgInst.AttackType == null)
        {
            eventHandler.DealDamage.Invoke(dmgInst);
            target.eventHandler.ReceiveDamage.Invoke(dmgInst);

            target.healthHandler.Health -= dmgInst.Amount * dmgInst.Modifier;
            return;
        }

        eventHandler.Attacking.Invoke(dmgInst);
        target.eventHandler.Attacked.Invoke(dmgInst);

        if (HelperMethods.CheckChance(critChance))
        {
            dmgInst.IsCrit = true;
        }

        if (!dmgInst.CannotBeDefended && dmgInst.IsDefended)
        {
            eventHandler.Defended.Invoke(dmgInst);
            target.eventHandler.Defending.Invoke(dmgInst);
        }

        if (!dmgInst.CannotMiss && dmgInst.IsMiss)
        {
            eventHandler.Missed.Invoke(dmgInst);
            target.eventHandler.Missing.Invoke(dmgInst);
        }
        else
        {
            if (!dmgInst.CannotCrit && dmgInst.IsCrit)
            {
                dmgInst.Amount *= critMultiplier;

                eventHandler.Critted.Invoke(dmgInst);
                target.eventHandler.Critting.Invoke(dmgInst);
            }
            eventHandler.DealDamage.Invoke(dmgInst);
            target.eventHandler.ReceiveDamage.Invoke(dmgInst);

            dmgInst.Amount -= healthHandler.Armor;
            
            target.healthHandler.Health -= dmgInst.Amount * dmgInst.Modifier;
        }
    }
}
