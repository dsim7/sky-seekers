using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityEffectMakeTargetable : AbilityEffect
{
    public override void TakeEffect(Character caster, Character target)
    {
        target.MakeTargetable();
    }
}
