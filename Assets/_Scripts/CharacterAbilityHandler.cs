using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAbilityHandler
{
    Character owner;
    public Character Character => owner;

    BoolVariable casting = new BoolVariable();
    public bool Casting => casting.Value;
    public void ListenToCasting(UnityAction listener) { casting.RegisterPostchangeEvent(listener); }
    public void UnlistenToCasting(UnityAction listener) { casting.UnregisterPostchangeEvent(listener); }

    public float Power { get; set; }
    public float SpellPower { get; set; }

    Dictionary<AbilityType, Ability> abilities;

    public CharacterAbilityHandler(Character character)
    {
        owner = character;
        Power = character.Template.Attack;
        SpellPower = character.Template.Spellpower;

        abilities = new Dictionary<AbilityType, Ability>();
        foreach (AbilityTemplate template in character.Template.Abilities)
        {
            if (abilities.ContainsKey(template.Type))
            {
                Debug.LogError("Character has multiple abilities of the same type. Replacing. " + template.Type);
            }
            else
            {
                abilities.Add(template.Type, new Ability(template, this));
            }
        }
    }

    public Ability GetAbility(AbilityType type)
    {
        if (abilities.ContainsKey(type))
        {
            return abilities[type];
        }
        return null;
    }

    public bool CastAbility(AbilityType abilityType, TargetingAI aiTargeting = null)
    {
        if (!owner.Available)
        {
            Debug.Log("character busy, not active, or incapacitated");
            return false;
        }
        if (casting.Value)
        {
            Debug.Log("already casting a spell");
            return false;
        }

        Ability ability = GetAbility(abilityType);
        if (ability == null)
        {
            Debug.LogError("Ability doesn't exist");
            return false;
        }
        if (!ability.CanCast)
        {
            Debug.Log("Ability can't cast");
            return false;
        }

        AbilityInstance abInst = ability.GetNewInstance(owner);

        abInst.OnStart.AddListener(() =>
        {
            casting.Value = true;
            owner.ActionPointHandler.UseActionPoints(ability.Template.ActionPointCost);
        });

        abInst.OnComplete.AddListener(() => 
        {
            casting.Value = false;
            owner.Mover.DoneInMelee();
        });

        abInst.OnCancel.AddListener(() =>
        {
            casting.Value = false;
            owner.Mover.DoneInMelee();
        });
        
        ability.StartCasting(aiTargeting);
        return true;
    }

    public void TickCooldowns()
    {
        foreach (var abilityKey in abilities)
        {
            abilityKey.Value.Cooldown.TickCD();
        }
    }
}
