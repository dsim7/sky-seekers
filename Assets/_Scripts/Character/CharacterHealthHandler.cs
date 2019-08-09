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

    bool dead;
    public bool Dead
    {
        get { return dead; }

        set
        {
            dead = value;
            if (dead)
            {
                health = 0;
                Owner.Actor.GetComponent<Animator>().SetTrigger("Dead");
                Owner.StatusHandler.ClearStatuses();
                if (Owner.PositionHandler.IsLastMelee && Owner.Team.SupportCharacters.Count != 0)
                {
                    Owner.Team.SupportCharacters[0].PositionHandler.Reposition();
                }
            }

            Changed();

        }
    }

    public CharacterHealthHandler(Character owner)
    {
        Owner = owner;
        MaxHealth = owner.Template.MaxHealth;
        Health = owner.Template.MaxHealth;
    }

    public void DealDamage(DamageInstance dmgInst)
    {
        if (dead)
        {
            return;
        }

        Character caster = dmgInst.Dealer;
        Character target = dmgInst.Target;

        dmgInst.Amount *= Mathf.Max(1 + ((caster.AbilityHandler.Power - 10) * 0.075f), 0);

        if (dmgInst.AttackType == null)
        {
            caster.EventHandler.DealDamage.Invoke(dmgInst);
            target.EventHandler.ReceiveDamage.Invoke(dmgInst);

            float finalDmg = dmgInst.Amount * dmgInst.Modifier;
            target.HealthHandler.Health -= finalDmg;
            Owner.FloatingTextHandler.ShowText(target.Actor.transform, finalDmg.ToString("#"), dmgInst.DamageType.Color);
            return;
        }

        caster.EventHandler.Attacking.Invoke(dmgInst);
        target.EventHandler.Attacked.Invoke(dmgInst);

        if ((dmgInst.CritChanceOverride != -1f && HelperMethods.CheckChance(dmgInst.CritChanceOverride)) ||
            HelperMethods.CheckChance(caster.CritChance))
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
                dmgInst.Amount *= dmgInst.CritMultiplierOverride != -1f ? dmgInst.CritMultiplierOverride : caster.CritMultiplier;

                caster.EventHandler.Critted.Invoke(dmgInst);
                target.EventHandler.Critting.Invoke(dmgInst);
            }
            caster.EventHandler.DealDamage.Invoke(dmgInst);
            target.EventHandler.ReceiveDamage.Invoke(dmgInst);

            float finalDmg = dmgInst.Amount * dmgInst.Modifier;
            dmgInst.Amount -= caster.Defense;

            target.HealthHandler.Health -= Mathf.Max(1, finalDmg);
            Owner.FloatingTextHandler.ShowText(target.Actor.transform, finalDmg.ToString("#"), dmgInst.DamageType.Color);
        }
    }
}
