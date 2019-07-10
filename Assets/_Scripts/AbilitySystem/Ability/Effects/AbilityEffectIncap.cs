using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityEffectIncap : AbilityEffect
{
    public override void TakeEffect(Character caster, Character target)
    {
        target.Incapacitate();
    }
}
