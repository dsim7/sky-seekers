using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ModGenRemoveStatusOnAttack : ModifierGenerator
{
    [SerializeField]
    float chance;

    public override ModifierBase GenerateInstance()
    {
        return new ModRemoveStatusOnAttack(chance);
    }

    class ModRemoveStatusOnAttack : Modifier<DamageInstance>
    {
        float chance;

        public ModRemoveStatusOnAttack(float chance)
        {
            this.chance = chance;
        }

        protected override UnityEvent<DamageInstance> GetEvent(CharacterEventHandler eventHandler)
        {
            return eventHandler.Attacking;
        }

        protected override void Modify(DamageInstance instance)
        {
            if (chance == 1 || HelperMethods.CheckChance(chance))
            {
                OriginStatusEffect.Remove();
            }
        }
    }
}
