using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    ModifierGenerator[] modifiers;
    [Space]
    [SerializeField]
    AbilityOnHitEffect[] onApply;
    [SerializeField]
    AbilityOnHitEffect[] onRemove;
    [SerializeField]
    AbilityOnHitEffect[] onTick;
    [Header("Special Effects")]
    [SerializeField]
    SpecialEffectGenerator[] persistentSfx;
    [SerializeField]
    SpecialEffectGenerator[] onApplySfx;
    [SerializeField]
    SpecialEffectGenerator[] onTickSfx;
    [SerializeField]
    SpecialEffectGenerator[] onRemoveSfx;

    public int Duration => duration;
    public int TickRate => tickRate;
    public bool Stacks => stacks;
    public ModifierGenerator[] Modifiers => modifiers;
    public AbilityOnHitEffect[] OnApply => onApply;
    public AbilityOnHitEffect[] OnRemove => onRemove;
    public AbilityOnHitEffect[] OnTick => onTick;
    public SpecialEffectGenerator[] PersistentSFX => persistentSfx;
    public SpecialEffectGenerator[] OnApplySFX => onApplySfx;
    public SpecialEffectGenerator[] OnTickSFX => onTickSfx;
    public SpecialEffectGenerator[] OnRemoveSFX => onRemoveSfx;
}
