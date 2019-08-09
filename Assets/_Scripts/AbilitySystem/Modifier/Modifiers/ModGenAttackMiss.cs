using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ModGenAttackMiss : ModifierGenerator
{
    [SerializeField]
    float chance;
    [SerializeField]
    AttackType type;
    [SerializeField]
    IncomingOrOutgoing incomingOrOutgoing;

    public override ModifierBase GenerateInstance()
    {
        return new ModAttackMiss(chance, type, incomingOrOutgoing);
    }

    class ModAttackMiss : Modifier<DamageInstance>
    {
        float chance;
        AttackType attackType;
        IncomingOrOutgoing incomingOrOutgoing;

        public ModAttackMiss(float chance, AttackType attackType, IncomingOrOutgoing incomingOrOutgoing)
        {
            this.chance = chance;
            this.attackType = attackType;
            this.incomingOrOutgoing = incomingOrOutgoing;
        }

        protected override UnityEvent<DamageInstance> GetEvent(CharacterEventHandler eventHandler)
        {
            return incomingOrOutgoing == IncomingOrOutgoing.Incoming ? eventHandler.Attacked : eventHandler.Attacking;
        }

        protected override void Modify(DamageInstance instance)
        {
            if (instance.AttackType != null && (attackType == null || instance.AttackType == attackType) && HelperMethods.CheckChance(chance))
            {
                instance.IsMiss = true;
                OriginStatusEffect.RemoveStack();
            }
        }
    }

    public enum IncomingOrOutgoing
    {
        Incoming, Outgoing
    }
}
