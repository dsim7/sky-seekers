using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityEffectGainAP : AbilityEffect
{
    [SerializeField]
    int apGained;
    [SerializeField]
    float chance;

    public override void TakeEffect(Character caster, Character target)
    {
        caster.ActionPointHandler.UseActionPoints(-1 * apGained);
    }
}
