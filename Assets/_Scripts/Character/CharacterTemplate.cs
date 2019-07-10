using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterTemplate : ScriptableObject
{
    [SerializeField]
    float maxHealth, attack, defense, spellpower;
    [SerializeField]
    List<AbilityTemplate> abilities;
    [SerializeField]
    int startingActionPoints;
    [SerializeField]
    CharacterAI ai;
    
    public float MaxHealth => maxHealth;
    public float Attack => attack;
    public float Defense => defense;
    public float Spellpower => spellpower;
    public List<AbilityTemplate> Abilities => abilities;
    public int StartingActionPoints => startingActionPoints;
    public CharacterAI AI => ai;
}
