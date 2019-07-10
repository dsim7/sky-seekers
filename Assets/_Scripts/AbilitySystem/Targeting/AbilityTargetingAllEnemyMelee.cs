using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class AbilityTargetingAllEnemyMelee : AbilityTargeting
{
    public override bool IsAutoTarget => true;

    public override void AutoTarget(AbilityInstance abilityInstance)
    {
        List<Character> enemyCharacters = abilityInstance.Caster.Team.EnemyTeam.Characters;
        
        abilityInstance.Targets.AddRange(enemyCharacters.Where(
            character => character.PositionHandler.Position == TeamPosition.Melee));
    }
}
