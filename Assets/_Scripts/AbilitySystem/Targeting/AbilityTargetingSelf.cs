using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class AbilityTargetingSelf : AbilityTargeting
{
    public override bool IsAutoTarget => true;

    public override void AutoTarget(AbilityInstance abilityInstance)
    {
        abilityInstance.Targets.Add(abilityInstance.Caster);
    }
}
