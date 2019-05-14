using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class PassiveFortified : PassiveEffect<DamageInstance>
{
    [SerializeField]
    float damagePercentReduction;

    protected override void Effect(DamageInstance instance)
    {
        instance.Modifier *= 1 - damagePercentReduction;
    }

    protected override UnityEvent<DamageInstance> GetEvent(CharacterEventHandler eventHandler)
    {
        return eventHandler.ReceiveDamage;
    }
}
