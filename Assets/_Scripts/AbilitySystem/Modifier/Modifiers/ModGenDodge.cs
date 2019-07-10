using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ModGenDodge : ModifierGenerator
{
    [SerializeField]
    float chance;
    [SerializeField]
    AttackType type;

    public override ModifierBase GenerateInstance()
    {
        return new ModDodge(chance, type);
    }

    class ModDodge : Modifier<DamageInstance>
    {
        float chance;
        AttackType attackType;

        public ModDodge(float chance, AttackType attackType)
        {
            this.chance = chance;
            this.attackType = attackType;
        }

        protected override UnityEvent<DamageInstance> GetEvent(CharacterEventHandler eventHandler)
        {
            return eventHandler.Attacked;
        }

        protected override void Modify(DamageInstance instance)
        {
            if ((attackType == null || instance.AttackType == attackType) && HelperMethods.CheckChance(chance))
            {
                instance.IsMiss = true;
                OriginStatusEffect.RemoveStack();
            }
        }
    }
}
