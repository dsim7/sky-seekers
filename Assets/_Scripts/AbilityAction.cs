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
    SpecialEffectPoolRef[] targetSfxs;
    [SerializeField]
    SpecialEffectPoolRef[] casterSfxs;

    public float DelayToNextAction => delayToNextAction;

    public void DoAction(Character caster, Character[] targets)
    {
        if (isMelee)
        {
            caster.Actor.MoveToMelee(targets[indexOfPrimaryTarget].Actor);
        }

        if (animationName != default(string) && caster.Actor.HasAnimation(animationName))
        {
            caster.Actor.PlayAnimation(animationName);

            caster.Actor.SetCurrentAnimationHitEffect(() =>
            {
                DoEffects(caster, targets);
                DoSFXs(caster, targets);
            });
        }
        else
        {
            DoEffects(caster, targets);
            DoSFXs(caster, targets);
        }
    }

    void DoEffects(Character caster, Character[] targets)
    {
        if (isAoe)
        {
            foreach (AbilityOnHitEffect effect in effects)
            {
                foreach (Character target in targets)
                {
                    effect.TakeEffect(caster, target);
                }
            }
        }
        else
        {
            foreach (AbilityOnHitEffect effect in effects)
            {
                effect.TakeEffect(caster, targets[indexOfPrimaryTarget]);
            }
        }
    }

    void DoSFXs(Character caster, Character[] targets)
    {
        foreach (SpecialEffectPoolRef sfx in casterSfxs)
        {
            sfx.Value.GenerateSFX(caster.Actor.transform);
        }

        if (isAoe)
        {
            foreach (SpecialEffectPoolRef sfx in targetSfxs)
            {
                foreach (Character target in targets)
                {
                    sfx.Value.GenerateSFX(target.Actor.transform);
                }
            }
        }
        else
        {
            foreach (SpecialEffectPoolRef sfx in targetSfxs)
            {
                sfx.Value.GenerateSFX(targets[indexOfPrimaryTarget].Actor.transform);
            }
        }
    }
}
