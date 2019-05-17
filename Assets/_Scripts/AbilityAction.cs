using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityAction : ScriptableObject
{ 
    [SerializeField]
    string animationName;
    [SerializeField]
    bool isMelee;
    [SerializeField]
    bool isAoe;
    [SerializeField]
    int indexOfPrimaryTarget;
    [SerializeField]
    float delayToNextAction;
    [Header("Functional Effects")]
    [SerializeField]
    AbilityOnHitEffect[] effects;
    [Header("Special Effects")]
    [SerializeField]
    SpecialEffectGenerator[] targetSfxs;
    [SerializeField]
    SpecialEffectGenerator[] casterSfxs;

    public string AnimationName => animationName;
    public bool IsMelee => isMelee;
    public int IndexOfPrimaryTarget => indexOfPrimaryTarget;
    public float DelayToNextAction => delayToNextAction;

    public void DoAction(Character caster, List<Character> targets)
    {
        DoEffects(caster, targets);
        DoSFXs(caster, targets);
    }

    void DoEffects(Character caster, List<Character> targets)
    {
        if (isAoe)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                for (int j = 0; j < targets.Count; j++)
                {
                    effects[i].TakeEffect(caster, targets[j]);
                }
            }
        }
        else
        {
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i].TakeEffect(caster, targets[indexOfPrimaryTarget]);
            }
        }
    }

    void DoSFXs(Character caster, List<Character> targets)
    {
        for (int i = 0; i < casterSfxs.Length; i++)
        {
            casterSfxs[i].Value.GenerateSFX(caster.Actor.transform);
        }

        if (isAoe)
        {
            for (int i = 0; i < targetSfxs.Length; i++)
            {
                for (int j = 0; j < targets.Count; j++)
                {
                    targetSfxs[i].Value.GenerateSFX(targets[j].Actor.transform);
                }
            }
        }
        else
        {
            for (int i = 0; i < targetSfxs.Length; i++)
            {
                targetSfxs[i].Value.GenerateSFX(targets[indexOfPrimaryTarget].Actor.transform);
            }
        }
    }
}
