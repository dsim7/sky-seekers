using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterPositionHandler
{
    public const float REPOSITION_DELAY = 0.5f;

    public Character Character { get; }
    public int CostToReposition { get; } = 1;
    public bool IsLastMelee { get; private set; }
    public CharacterEvent OnReposition { get; } = new CharacterEvent();

    int preventingReposition;

    TeamPositionVariable position = new TeamPositionVariable();
    public TeamPosition Position => position.Value;
    public void ListenToPosition(UnityAction listener) { position.RegisterPostchangeEvent(listener); }
    public void UnlistenToPosition(UnityAction listener) { position.UnregisterPostchangeEvent(listener); }

    BoolVariable canDoRepositionAction = new BoolVariable();
    public bool CanDoRepositionAction => canDoRepositionAction.Value;
    public void ListenToCanDoRepositionAction(UnityAction listener) { canDoRepositionAction.RegisterPostchangeEvent(listener); }
    public void UnlistenToCanDoRepositionAction(UnityAction listener) { canDoRepositionAction.UnregisterPostchangeEvent(listener); }

    BoolVariable canReposition = new BoolVariable();
    public bool CanReposition => canReposition.Value;
    public void ListenToCanReposition(UnityAction listener) { canReposition.RegisterPostchangeEvent(listener); }
    public void UnlistenToCanReposition(UnityAction listener) { canReposition.UnregisterPostchangeEvent(listener); }

    BoolVariable isRepositioning = new BoolVariable();
    public bool IsRepositioning => isRepositioning.Value;
    public void ListenToIsRepositioning(UnityAction listener) { isRepositioning.RegisterPostchangeEvent(listener); }
    public void UnlistenToIsRepositioning(UnityAction listener) { isRepositioning.UnregisterPostchangeEvent(listener); }
    
    public CharacterPositionHandler(Character character, TeamPosition startingPosition)
    {
        Character = character;
        position.Value = startingPosition;
        RegisterCanRepositionListeners();
    }

    public void DoRepositionAction()
    {
        Character.ActionPointHandler.UseActionPoints(CostToReposition);
        Reposition();
    }

    public void Reposition()
    {
        isRepositioning.Value = true;
        position.Value = position.Value == TeamPosition.Melee ? TeamPosition.Support : TeamPosition.Melee;
        OnReposition.Invoke(Character);
        PreventLastMeleeFromRepositioning();

        Timer.RunTimerOnce(REPOSITION_DELAY, () => isRepositioning.Value = false);
    }

    public void PreventReposition()
    {
        preventingReposition += 1;
        if (preventingReposition == 1)
        {
            UpdateCanDoReposition();
        }
    }

    public void RemoveRepositionPrevention()
    {
        preventingReposition -= 1;
        if (preventingReposition == 0)
        {
            UpdateCanDoReposition();
        }
    }

    void PreventLastMeleeFromRepositioning()
    {
        List<Character> teamMelee = Character.Team.MeleeCharacters;

        int targetableMeleeCount = 0;
        Character lastMelee = null;
        for (int i = 0; i < teamMelee.Count; i++)
        {
            if (teamMelee[i].Targetable)
            {
                targetableMeleeCount++;
                lastMelee = teamMelee[i];
            }
        }

        if (targetableMeleeCount == 1)
        {
            CharacterPositionHandler lastMeleeHandler = lastMelee.PositionHandler;
            lastMeleeHandler.IsLastMelee = true;
            lastMeleeHandler.UpdateCanDoReposition();
        }
        else
        {
            for (int i = 0; i < teamMelee.Count; i++)
            {
                CharacterPositionHandler charHandler = teamMelee[i].PositionHandler;
                if (charHandler.IsLastMelee)
                {
                    charHandler.IsLastMelee = false;
                    charHandler.UpdateCanDoReposition();
                }
            }
        }
    }

    void RegisterCanRepositionListeners()
    {
        Character.ListenToAvailable(UpdateCanDoReposition);
        Character.ActionPointHandler.ListenToPoints(UpdateCanDoReposition);

        Character.ListenToTargetable(PreventLastMeleeFromRepositioning);
    }

    void UpdateCanDoReposition()
    {
        bool canRepositionResult = !IsLastMelee && preventingReposition == 0;
        if (canRepositionResult != canReposition.Value)
        {
            canReposition.Value = canRepositionResult;
        }
        bool canDoRepositionActionResult = canRepositionResult && Character.Available &&
            Character.ActionPointHandler.HavePoints(CostToReposition);
        if (canDoRepositionActionResult != canDoRepositionAction.Value)
        {
            canDoRepositionAction.Value = canDoRepositionActionResult;
        }
    }
}

public class TeamPositionVariable : ObservedVariable<TeamPosition> { }