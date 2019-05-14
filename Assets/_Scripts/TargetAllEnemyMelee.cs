using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class TargetAllEnemyMelee : AbilityTargeting
{
    public override void AutoTarget(AbilityInstance abilityInstance)
    {
        Character[] enemyCharacters = abilityInstance.Caster.TeamHandler.Team.EnemyTeam.Characters;
        
        abilityInstance.Targets.AddRange(enemyCharacters.Where(
            character => character.TeamHandler.Position == TeamPosition.Melee));
    }

    public override bool SelectTargetCheck(Character selected, AbilityInstance abilityInstance)
    {
        return true;
    }
}
