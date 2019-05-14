using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityTargeting : ScriptableObject
{
    [SerializeField]
    bool instantExecution;
    [SerializeField]
    int requiredTargets;

    public bool IsAutoTarget => instantExecution;
    public int NumTargetsRequired => requiredTargets;

    public abstract void AutoTarget(AbilityInstance abilityInstance);

    public abstract bool SelectTargetCheck(Character selected, AbilityInstance abilityInstance);
}
