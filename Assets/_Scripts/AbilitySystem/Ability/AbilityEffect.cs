using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffect : ScriptableObject
{
    public abstract void TakeEffect(Character caster, Character target);
}
