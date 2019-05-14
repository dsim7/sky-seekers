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
}
