using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class TargetSelf : AbilityTargeting
{
    public override void AutoTarget(AbilityInstance abilityInstance)
    {
        abilityInstance.Targets.Add(abilityInstance.Caster);
    }
}
