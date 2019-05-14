using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilityHandler
{
    Character owner;
    Dictionary<AbilityType, Ability> abilities;

    public float Power { get; set; }
    public float SpellPower { get; set; }

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
                Debug.LogWarning("Character has multiple abilities of the same type. Replacing.");
            }
            abilities.Add(template.Type, new Ability(template));
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

    public AbilityInstance CastAbility(AbilityType type, List<Character> AItargets = null)
    {
        if (type == null)
        {
            Debug.LogWarning("No Ability Type");
            return null;
        }

        if (!abilities.ContainsKey(type))
        { 
            Debug.LogWarning("Ability Set does not contain " + type.name);
            return null;
        }
        Ability ability = abilities[type];
        return CastAbility(ability, AItargets);
    }

    public AbilityInstance CastAbility(Ability ability, List<Character> AItargets = null)
    {
        AbilityInstance abilityInstance = ability.Cast(owner, AItargets);
        return abilityInstance;
    }

    public void TickCooldowns()
    {
        foreach (var abilityKey in abilities)
        {
            abilityKey.Value.Cooldown.TickCD();
        }
    }
}
