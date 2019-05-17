using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class ModGenEffectOnAttacked : ModifierGenerator
{
    [SerializeField]
    AbilityOnHitEffect effect;
    [SerializeField]
    float chance;

    public override ModifierBase GenerateInstance()
    {
        return new ModEffectOnAttacked(effect, chance);
    }

    class ModEffectOnAttacked : Modifier<DamageInstance>
    {
        AbilityOnHitEffect effect;
        float chance;

        public ModEffectOnAttacked(AbilityOnHitEffect effect, float chance)
        {
            this.effect = effect;
            this.chance = chance;
        }

        protected override UnityEvent<DamageInstance> GetEvent(CharacterEventHandler eventHandler)
        {
            return eventHandler.Attacked;
        }

        protected override void Modify(DamageInstance instance)
        {
            if (chance == 1 || HelperMethods.CheckChance(chance))
            {
                effect.TakeEffect(OriginStatusEffect.Caster, instance.Dealer);
            }
        }
    }
}
