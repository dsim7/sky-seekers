using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityEffectMakeUntargetable : AbilityEffect
{
    public override void TakeEffect(Character caster, Character target)
    {
        target.MakeUntargetable();
    }
}