using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ModGenModDmg : ModifierGenerator
{
    [SerializeField]
    DealtOrReceive dealtOrReceive;
    [SerializeField]
    float damagePercent;
    [SerializeField]
    float damageConstant;
    [SerializeField]
    float chance = 1f;

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
        return new ModDamageReduction(dealtOrReceive, damagePercent, damageConstant, damageType, chance,
            mustBeAttack, attackType, attackMustHit, attackMustBeDefended, attackMustCrit);
    }

    class ModDamageReduction : Modifier<DamageInstance>
    {
        DealtOrReceive dealtOrReceive;
        float damagePercent, damageConstant;
        DamageType damageType;
        float chance;

        bool mustBeAttack;
        AttackType attackType;
        bool attackMustHit, attackMustBeDefended, attackMustCrit;

        public ModDamageReduction(DealtOrReceive dealtOrReceive, float damagePercent, float damageConstant, DamageType damageType, float chance,
            bool mustBeAttack, AttackType attackType, bool attackMustHit, bool attackMustBeDefended, bool attackMustCrit)
        {
            this.dealtOrReceive = dealtOrReceive;
            this.damageType = damageType;
            this.damagePercent = damagePercent;
            this.damageConstant = damageConstant;
            this.chance = chance;
            this.mustBeAttack = mustBeAttack;
            this.attackType = attackType;
            this.attackMustHit = attackMustHit;
            this.attackMustBeDefended = attackMustBeDefended;
            this.attackMustCrit = attackMustCrit;
        }

        protected override void Modify(DamageInstance instance)
        {
            if (HelperMethods.CheckChance(chance) && 
                DetermineApplicable(instance))
            {
                instance.Amount *= damagePercent;
                instance.Amount += damageConstant;
                OriginStatusEffect.RemoveStack();
            }
        }

        protected override UnityEvent<DamageInstance> GetEvent(CharacterEventHandler eventHandler)
        {
            return dealtOrReceive == DealtOrReceive.Dealt ? eventHandler.DealDamage : eventHandler.ReceiveDamage;
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

    enum DealtOrReceive
    {
        Dealt, Receive
    }
}
