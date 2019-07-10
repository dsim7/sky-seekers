using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ModGenEffectOnDmg : ModifierGenerator
{
    [SerializeField]
    AttackOrAttacked onAttackOrAttacked;
    [SerializeField]
    EffectTarget effectTarget;
    [SerializeField]
    AbilityEffect[] effects;
    [SerializeField]
    float chance = 1;

    [SerializeField]
    DamageType damageType;
    [SerializeField]
    bool mustBeAttack;
    [SerializeField]
    AttackType attackType;
    [SerializeField]
    bool attackMustHit, attackMustBeDefended, attackMustCrit;

    public override ModifierBase GenerateInstance()
    {
        return new ModEffectOnAttacking(onAttackOrAttacked, effects, chance, effectTarget, mustBeAttack, 
            attackMustHit, attackType, damageType, attackMustBeDefended, attackMustCrit);
    }

    class ModEffectOnAttacking : Modifier<DamageInstance>
    {
        AttackOrAttacked onAttackOrAttacked;
        AbilityEffect[] effects;
        float chance;
        EffectTarget effectTarget;
        DamageType damageType;

        bool mustBeAttack;
        AttackType attackType;
        bool attackMustHit, attackMustBeDefended, attackMustCrit;

        public ModEffectOnAttacking(AttackOrAttacked onAttackOrAttacked, AbilityEffect[] effects, float chance, EffectTarget effectTarget, bool mustBeAttack, bool attackMustHit,
            AttackType attackType, DamageType damageType, bool attackMustBeDefended, bool attackMustCrit)
        {
            this.onAttackOrAttacked = onAttackOrAttacked;
            this.effects = effects;
            this.chance = chance;
            this.effectTarget = effectTarget;
            this.mustBeAttack = mustBeAttack;
            this.attackMustHit = attackMustHit;
            this.attackMustBeDefended = attackMustBeDefended;
            this.attackMustCrit = attackMustCrit;
            this.attackType = attackType;
            this.damageType = damageType;
        }

        protected override UnityEvent<DamageInstance> GetEvent(CharacterEventHandler eventHandler)
        {
            return onAttackOrAttacked == AttackOrAttacked.Attacking ? eventHandler.Attacking : eventHandler.Attacked;
        }

        protected override void Modify(DamageInstance instance)
        {
            if (HelperMethods.CheckChance(chance) &&
                (chance == 1 || HelperMethods.CheckChance(chance)))
            {
                Character eftTrgt;
                switch (effectTarget)
                {
                    case EffectTarget.Caster: eftTrgt = OriginStatusEffect.Caster;
                        break;
                    case EffectTarget.Attacker: eftTrgt = instance.Dealer;
                        break;
                    default: eftTrgt = instance.Target;
                        break;
                }
                for (int i = 0; i < effects.Length; i++)
                {
                    effects[i].TakeEffect(OriginStatusEffect.Caster, eftTrgt);
                }
            }
        }

        bool DetermineApplicable(DamageInstance instance)
        {
            if (damageType != null && instance.DamageType != damageType)
            {
                return false;
            }
            if (mustBeAttack)
            {
                if (instance.AttackType == null)
                {
                    return false;
                }
                if (attackMustHit && instance.IsMiss)
                {
                    return false;
                }
                if (attackMustBeDefended && !instance.IsDefended)
                {
                    return false;
                }
                if (attackMustCrit && !instance.IsCrit)
                {
                    return false;
                }
            }
            return true;
        }
    }

    enum EffectTarget
    {
        Caster, Attacker, Attackee
    }

    enum AttackOrAttacked
    {
        Attacking, Attacked
    }
}
