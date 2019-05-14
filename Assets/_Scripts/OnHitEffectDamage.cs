using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OnHitEffectDamage : AbilityOnHitEffect
{
    [SerializeField]
    float baseDamage;
    [SerializeField]
    AttackType attackType;
    [SerializeField]
    DamageType damageType;

    public override void TakeEffect(Character caster, Character target)
    {
        DamageInstance.NewAttack(caster, target, baseDamage, damageType, attackType).DoDamage();
    }
}
