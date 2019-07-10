using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class TargetingAI : ScriptableObject
{
    [SerializeField]
    TargetingPriorities firstPriority, secondPriority;
    [SerializeField]
    [Range(0f,1f)]
    float priorityDeviation = 0;

    List<Character> tempTargets = new List<Character>();
    
    public List<Character> GetAITargets(AbilityInstance abInst)
    {
        AbilityTargeting targeting = abInst.Ability.Template.Targeting;
        List<Character> candidates = targeting.GetCandidates(abInst.Caster);
        int numTargetsReq = targeting.NumTargetsRequired;

        if (candidates.Count == 0 || (candidates.Count < numTargetsReq && targeting.CannotRepeatSelection))
        {
            return null;
        }

        List<Character> prioritizedCandidates = candidates.OrderByDescending(GetSelector(firstPriority)).
            ThenBy(GetSelector(secondPriority)).ToList();

        tempTargets.Clear();
        for (int i = 0; i < numTargetsReq; i++)
        {
            for (int j = 0; j < prioritizedCandidates.Count; j++)
            {
                if (j == prioritizedCandidates.Count - 1 || HelperMethods.CheckChance(1 - priorityDeviation))
                {
                    tempTargets.Add(prioritizedCandidates[j]);
                    if (targeting.CannotRepeatSelection)
                    {
                        tempTargets.Remove(prioritizedCandidates[j]);
                    }
                    break;
                }
            }
        }
        return tempTargets;
    }

    Func<Character, float> GetSelector(TargetingPriorities priority)
    {
        switch (priority)
        {
            case TargetingPriorities.HighThreat: return c => c.Threat + 1;
            case TargetingPriorities.HighThreatened: return c => c.Threatened;
            default: return c => 0;
        }
    }
}

public enum TargetingPriorities
{
    None, HighThreat, HighThreatened
}