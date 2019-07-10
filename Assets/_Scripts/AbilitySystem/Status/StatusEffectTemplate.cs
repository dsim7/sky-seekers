using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class StatusEffectTemplate : ScriptableObject
{
    [SerializeField]
    int duration = 1;
    [SerializeField]
    int tickRate;
    [SerializeField]
    bool allowMultiple;
    [SerializeField]
    int maxStacks = 1;
    [SerializeField]
    int startingStacks = 1;
    [Header("Functional Effects")]
    [SerializeField]
    ModifierGenerator[] modifiers;
    [Space]
    [SerializeField]
    AbilityEffect[] onApply;
    [SerializeField]
    AbilityEffect[] onRemove;
    [SerializeField]
    AbilityEffect[] onTick;
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
    public bool AllowMultiple => allowMultiple;
    public int MaxStacks => maxStacks;
    public int StartingStacks => startingStacks;
    public ModifierGenerator[] Modifiers => modifiers;
    public AbilityEffect[] OnApply => onApply;
    public AbilityEffect[] OnRemove => onRemove;
    public AbilityEffect[] OnTick => onTick;
    public SpecialEffectGenerator[] PersistentSFX => persistentSfx;
    public SpecialEffectGenerator[] OnApplySFX => onApplySfx;
    public SpecialEffectGenerator[] OnTickSFX => onTickSfx;
    public SpecialEffectGenerator[] OnRemoveSFX => onRemoveSfx;
}
