using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OnHitEffectApplyStatus : AbilityOnHitEffect
{
    [SerializeField]
    StatusEffectTemplate statusTemplate;

    public override void TakeEffect(Character caster, Character target)
    {
        caster.StatusHandler.ApplyStatus(statusTemplate, target);
    }
}
