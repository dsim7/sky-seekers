using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEventHandler
{
    Character owner;

    public AbilityEvent TargetedByAbility { get; private set; }
    public AbilityEvent CastAbility { get; private set; }
    public DamageEvent ReceiveDamage { get; private set; }
    public DamageEvent DealDamage { get; private set; }
    public DamageEvent Attacked { get; private set; }
    public DamageEvent Attacking { get; private set; }
    public DamageEvent Missed { get; private set; }
    public DamageEvent Missing { get; private set; }
    public DamageEvent Defended { get; private set; }
    public DamageEvent Defending { get; private set; }
    public DamageEvent Critted { get; private set; }
    public DamageEvent Critting { get; private set; }
    public StatusEvent StatusApplied { get; private set; }
    public StatusEvent StatusApplying { get; private set; }

    public CharacterEventHandler(Character owner)
    {
        this.owner = owner;

        TargetedByAbility = new AbilityEvent();
        CastAbility = new AbilityEvent();
        DealDamage = new DamageEvent();
        ReceiveDamage = new DamageEvent();
        Attacked = new DamageEvent();
        Attacking = new DamageEvent();
        Missed = new DamageEvent();
        Missing = new DamageEvent();
        Defended = new DamageEvent();
        Defending = new DamageEvent();
        Critted = new DamageEvent();
        Critting = new DamageEvent();
        StatusApplied = new StatusEvent();
        StatusApplying = new StatusEvent();
    }
    
    public class AbilityEvent : UnityEvent<AbilityInstance> { }

    public class DamageEvent : UnityEvent<DamageInstance> { }

    public class StatusEvent : UnityEvent<StatusEffectInstance> { }
}
