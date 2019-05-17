using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHealthHandler : ObservedObject
{
    public Character Owner { get; private set; }

    float maxHealth;
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; Changed(); } }

    float health;
    public float Health
    {
        get { return health; }

        set
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            Dead = health == 0;
            Changed();
        }
    }

    float defense;
    public float Armor { get { return defense; } set { defense = value; Changed(); } }

    bool dead;
    public bool Dead
    {
        get { return dead; }

        set
        {
            dead = value;
            health = dead ? 0 : health;
            Changed();
        }
    }

    public CharacterHealthHandler(Character owner)
    {
        Owner = owner;
        MaxHealth = owner.Template.MaxHealth;
        Health = owner.Template.MaxHealth;
        Armor = owner.Template.Defense;
    }

    public void DealDamage(DamageInstance dmgInst)
    {
        Character caster = dmgInst.Dealer;
        Character target = dmgInst.Target;

        dmgInst.Amount *= Mathf.Clamp(1 + ((caster.AbilityHandler.Power - 10) * 0.075f), 0, Mathf.Infinity);

        if (dmgInst.AttackType == null)
        {
            caster.EventHandler.DealDamage.Invoke(dmgInst);
            target.EventHandler.ReceiveDamage.Invoke(dmgInst);

            target.HealthHandler.Health -= dmgInst.Amount * dmgInst.Modifier;
            return;
        }

        caster.EventHandler.Attacking.Invoke(dmgInst);
        target.EventHandler.Attacked.Invoke(dmgInst);

        if (HelperMethods.CheckChance(caster.CritChance))
        {
            dmgInst.IsCrit = true;
        }

        if (!dmgInst.CannotBeDefended && dmgInst.IsDefended)
        {
            caster.EventHandler.Defended.Invoke(dmgInst);
            target.EventHandler.Defending.Invoke(dmgInst);
        }

        if (!dmgInst.CannotMiss && dmgInst.IsMiss)
        {
            caster.EventHandler.Missed.Invoke(dmgInst);
            target.EventHandler.Missing.Invoke(dmgInst);
        }
        else
        {
            if (!dmgInst.CannotCrit && dmgInst.IsCrit)
            {
                dmgInst.Amount *= caster.CritMultiplier;

                caster.EventHandler.Critted.Invoke(dmgInst);
                target.EventHandler.Critting.Invoke(dmgInst);
            }
            caster.EventHandler.DealDamage.Invoke(dmgInst);
            target.EventHandler.ReceiveDamage.Invoke(dmgInst);

            dmgInst.Amount -= caster.HealthHandler.Armor;

            target.HealthHandler.Health -= dmgInst.Amount * dmgInst.Modifier;
        }
    }
}
