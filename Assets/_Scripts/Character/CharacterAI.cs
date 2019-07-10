using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterAI : ScriptableObject
{
    [SerializeField]
    int turnPriority;
    public int TurnPriority => turnPriority;

    [Space]
    [SerializeField]
    bool preferMelee;
    [SerializeField]
    bool preferSupport;
    [SerializeField]
    [Range(0, 1)]
    float positioningPrioritization = 1;
    [Space]
    [SerializeField]
    AbilityType[] abilityTypesPrioritized;
    [SerializeField]
    [Range(0, 0.5f)]
    float abilityPrioritization = 0.5f;
    [SerializeField]
    TargetingAI targeting;

    public bool ConsiderRepositioning(Character character)
    {
        if (DetermineOutOfPosition(character) && HelperMethods.CheckChance(positioningPrioritization))
        {
            if (character.PositionHandler.CanDoRepositionAction)
            {
                character.PositionHandler.DoRepositionAction();
                return true;
            }
        }
        return false;
    }
    
    public bool ConsiderCastingAbility(Character character)
    {
        for (int i = 0; i < abilityTypesPrioritized.Length; i++)
        {
            if (HelperMethods.CheckChance(0.5f + abilityPrioritization))
            {
                if (character.AbilityHandler.GetAbility(abilityTypesPrioritized[i]).CanCast)
                {
                    bool castSuccessful = character.AbilityHandler.CastAbility(abilityTypesPrioritized[i], targeting);
                    if (castSuccessful)
                    {
                        return true;
                    }
                }
            }
        }

        for (int i = 0; i < abilityTypesPrioritized.Length; i++)
        {
            if (character.AbilityHandler.GetAbility(abilityTypesPrioritized[i]).CanCast)
            {
                bool castSuccessful = character.AbilityHandler.CastAbility(abilityTypesPrioritized[i], targeting);
                if (castSuccessful)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool DetermineOutOfPosition(Character character)
    {
        TeamPosition curPosition = character.PositionHandler.Position;
        return (preferMelee && curPosition != TeamPosition.Melee) ||
            (preferSupport && curPosition != TeamPosition.Support) ||
            (!preferSupport && !preferMelee);
    }
}
