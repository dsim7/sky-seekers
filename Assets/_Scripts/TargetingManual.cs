using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TargetingManual : AbilityTargeting
{
    [SerializeField]
    bool canSelectAllies, canSelectEnemies, canSelectSelf,
        canSelectMelee, canSelectSupport, cannotSelectLastMelee, cannotRepeatSelection;

    public override void AutoTarget(AbilityInstance abilityInstance)
    {
        return;
    }

    public override bool SelectTargetCheck(Character selected, AbilityInstance abilityInstance)
    {
        Character caster = abilityInstance.Caster;
        return (canSelectSelf || selected != caster) &&
            (canSelectAllies || selected.TeamHandler.Team != caster.TeamHandler.Team) &&
            (canSelectEnemies || selected.TeamHandler.Team == caster.TeamHandler.Team) &&
            (canSelectMelee || selected.TeamHandler.Position != TeamPosition.Melee) &&
            (canSelectSupport || selected.TeamHandler.Position != TeamPosition.Support) &&
            (!cannotSelectLastMelee || selected.TeamHandler.CanReposition) &&
            (!cannotRepeatSelection || !abilityInstance.Targets.Contains(selected));
    }
}
