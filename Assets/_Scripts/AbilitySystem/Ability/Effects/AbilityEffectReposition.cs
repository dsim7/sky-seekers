using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityEffectReposition : AbilityEffect
{
    public override void TakeEffect(Character caster, Character target)
    {
        caster.PositionHandler.Reposition();
    }
}
