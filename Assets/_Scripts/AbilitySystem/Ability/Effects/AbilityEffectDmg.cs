using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityEffectDmg : AbilityEffect
{
    [SerializeField]
    float baseDamage;
    [SerializeField]
    float powerScaling;
    [SerializeField]
    AttackType attackType;
    [SerializeField]
    DamageType damageType;
    [SerializeField]
    AbilityEffect effectOnHit;
    [Space]
    [SerializeField]
    float critChanceOverride = -1;
    [SerializeField]
    float critMultiplierOverride = -1;

    public override void TakeEffect(Character caster, Character target)
    {
        float scaledDamage = baseDamage + (caster.Power * powerScaling);
        DamageInstance attack = DamageInstance.NewAttack(caster, target, scaledDamage, damageType, attackType, critChanceOverride, critMultiplierOverride);
        attack.DoDamage();

        if (effectOnHit != null && !attack.IsMiss)
        {
            effectOnHit.TakeEffect(caster, target);
        }
    }
}
