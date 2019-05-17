using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TargetManual : AbilityTargeting
{
    public override bool IsAutoTarget => false;

    public override void AutoTarget(AbilityInstance abilityInstance)
    {
        return;
    }
}
