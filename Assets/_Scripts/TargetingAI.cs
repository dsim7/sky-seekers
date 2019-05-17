using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class TargetingAI : AbilityAITargeting
{
    [SerializeField]
    TargetingPriorities firstPriority, secondPriority;
    
    public override List<Character> GetAITargets(AbilityInstance abInst)
    {
        AbilityTargeting targeting = abInst.Ability.Template.Targeting;
        List<Character> candidates = targeting.GetCandidates(abInst.Caster);
        int numTargetsReq = targeting.NumTargetsRequired;

        if (candidates.Count == 0 || (candidates.Count < numTargetsReq && targeting.CannotRepeatSelection))
        {
            return null;
        }

        List<Character> targets = candidates.OrderByDescending(GetSelector(firstPriority)).
            ThenBy(GetSelector(secondPriority)).ToList();
        //targets.RemoveRange(numTargetsReq, targets.Count - numTargetsReq);
        return targets;
    }

    Func<Character, float> GetSelector(TargetingPriorities priority)
    {
        switch (priority)
        {
            case TargetingPriorities.HighThreat: return c => c.Threat;
            case TargetingPriorities.HighThreatened: return c => c.Threatened;
            default: return c => 0;
        }
    }
}

public enum TargetingPriorities
{
    None, HighThreat, HighThreatened
}