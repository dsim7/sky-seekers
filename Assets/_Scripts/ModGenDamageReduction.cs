using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ModGenDamageReduction : ModifierGenerator
{
    [SerializeField]
    float reductionPercent;

    public override ModifierBase GenerateInstance()
    {
        return new ModDamageReduction(reductionPercent);
    }

    class ModDamageReduction : Modifier<DamageInstance>
    {
        float reductionPercent;

        public ModDamageReduction(float reductionPercent)
        {
            this.reductionPercent = reductionPercent;
        }

        protected override void Modify(DamageInstance instance)
        {
            instance.Amount *= 1 - reductionPercent;
        }

        protected override UnityEvent<DamageInstance> GetEvent(CharacterEventHandler eventHandler)
        {
            return eventHandler.ReceiveDamage;
        }
    }
}