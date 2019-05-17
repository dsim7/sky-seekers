using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityAITargeting : ScriptableObject
{
    public abstract List<Character> GetAITargets(AbilityInstance absInst);
}
