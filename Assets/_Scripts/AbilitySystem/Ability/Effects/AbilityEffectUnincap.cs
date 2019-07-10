using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityEffectUnincap : AbilityEffect
{
    public override void TakeEffect(Character caster, Character target)
    {
        target.RemoveIncapacitation();
    }
}
