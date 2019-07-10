using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityEffectApplyStatus : AbilityEffect
{
    [SerializeField]
    StatusEffectTemplate statusTemplate;
    [SerializeField]
    float chanceToApply = 1;

    public override void TakeEffect(Character caster, Character target)
    {
        if (chanceToApply == 1 || HelperMethods.CheckChance(chanceToApply))
        {
            caster.StatusHandler.ApplyStatus(statusTemplate, target);
        }
    }
}
