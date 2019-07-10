using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEventHandler
{
    Character owner;

    public AbilityEvent TargetedByAbility { get; private set; } = new AbilityEvent();
    public AbilityEvent CastAbility { get; private set; } = new AbilityEvent();
    public DamageEvent ReceiveDamage { get; private set; } = new DamageEvent();
    public DamageEvent DealDamage { get; private set; } = new DamageEvent();
    public DamageEvent Attacked { get; private set; } = new DamageEvent();
    public DamageEvent Attacking { get; private set; } = new DamageEvent();
    public DamageEvent Missed { get; private set; } = new DamageEvent();
    public DamageEvent Missing { get; private set; } = new DamageEvent();
    public DamageEvent Defended { get; private set; } = new DamageEvent();
    public DamageEvent Defending { get; private set; } = new DamageEvent();
    public DamageEvent Critted { get; private set; } = new DamageEvent();
    public DamageEvent Critting { get; private set; } = new DamageEvent();
    public StatusEvent StatusApplied { get; private set; } = new StatusEvent();
    public StatusEvent StatusApplying { get; private set; } = new StatusEvent();

    public CharacterEventHandler(Character owner)
    {
        this.owner = owner;
    }
    
    public class AbilityEvent : UnityEvent<AbilityInstance> { }

    public class DamageEvent : UnityEvent<DamageInstance> { }

    public class StatusEvent : UnityEvent<StatusEffectInstance> { }
}
