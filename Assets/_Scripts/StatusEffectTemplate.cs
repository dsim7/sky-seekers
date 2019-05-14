using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatusEffectTemplate : ScriptableObject
{
    [SerializeField]
    int duration = 0;
    [SerializeField]
    int tickRate = 1;
    [SerializeField]
    bool stacks;
    [Header("Functional Effects")]
    [SerializeField]
    PassiveEffect[] persistentEffects;
    [Space]
    [SerializeField]
    AbilityOnHitEffect[] onApply;
    [SerializeField]
    AbilityOnHitEffect[] onRemove;
    [SerializeField]
    AbilityOnHitEffect[] onTick;
    [Header("Special Effects")]
    [SerializeField]
    SpecialEffectPoolRef[] persistentSfx;
    [SerializeField]
    SpecialEffectPoolRef[] onApplySfx;
    [SerializeField]
    SpecialEffectPoolRef[] onTickSfx;
    [SerializeField]
    SpecialEffectPoolRef[] onRemoveSfx;

    public int Duration => duration;
    public int TickRate => tickRate;
    public bool Stacks => stacks;
    public PassiveEffect[] PassiveEffects => persistentEffects;
    public AbilityOnHitEffect[] OnApply => onApply;
    public AbilityOnHitEffect[] OnRemove => onRemove;
    public AbilityOnHitEffect[] OnTick => onTick;
    public SpecialEffectPoolRef[] PersistentSFX => persistentSfx;
    public SpecialEffectPoolRef[] OnApplySFX => onApplySfx;
    public SpecialEffectPoolRef[] OnTickSFX => onTickSfx;
    public SpecialEffectPoolRef[] OnRemoveSFX => onRemoveSfx;
}
