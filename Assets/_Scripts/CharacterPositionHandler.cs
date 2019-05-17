using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterPositionHandler
{
    public const float REPOSITION_DELAY = 0.5f;

    Character owner;
    public Character Character => owner;
    
    int costToReposition = 1;
    public int CostToReposition => costToReposition;

    bool isLastMelee;
    public bool IsLastMelee => isLastMelee;

    int preventingReposition;

    TeamPositionVariable position = new TeamPositionVariable();
    public TeamPosition Position => position.Value;
    public void ListenToPosition(UnityAction listener) { position.RegisterPostchangeEvent(listener); }
    public void UnlistenToPosition(UnityAction listener) { position.UnregisterPostchangeEvent(listener); }
    
    BoolVariable canReposition = new BoolVariable();
    public bool CanReposition => canReposition.Value;
    public void ListenToCanReposition(UnityAction listener) { canReposition.RegisterPostchangeEvent(listener); }
    public void UnlistenToCanReposition(UnityAction listener) { canReposition.UnregisterPostchangeEvent(listener); }

    BoolVariable isRepositioning = new BoolVariable();
    public bool IsRepositioning => isRepositioning.Value;
    public void ListenToIsRepositioning(UnityAction listener) { isRepositioning.RegisterPostchangeEvent(listener); }
    public void UnlistenToIsRepositioning(UnityAction listener) { isRepositioning.UnregisterPostchangeEvent(listener); }

    CharacterEvent onReposition = new CharacterEvent();
    public CharacterEvent OnReposition => onReposition;

    public CharacterPositionHandler(Character character, TeamPosition startingPosition)
    {
        owner = character;
        position.Value = startingPosition;
        RegisterCanRepositionListeners();
    }

    public bool Reposition()
    {
        if (!CanReposition)
        {
            Debug.Log("Cannot reposition");
            return false;
        }
        isRepositioning.Value = true;
        owner.ActionPointHandler.UseActionPoints(CostToReposition);
        position.Value = position.Value == TeamPosition.Melee ? TeamPosition.Support : TeamPosition.Melee;
        onReposition.Invoke(owner);
        PreventLastMeleeFromRepositioning();

        Timer.RunTimerOnce(REPOSITION_DELAY, () => isRepositioning.Value = false);

        return true;
    }

    public void PreventReposition()
    {
        preventingReposition += 1;
        if (preventingReposition == 1)
        {
            UpdateCanReposition();
        }
    }

    public void RemoveRepositionPrevention()
    {
        preventingReposition -= 1;
        if (preventingReposition == 0)
        {
            UpdateCanReposition();
        }
    }

    void PreventLastMeleeFromRepositioning()
    {
        Character[] teamMelee = owner.Team.MeleeCharacters;
        if (teamMelee.Length == 1)
        {
            CharacterPositionHandler lastMeleeHandler = teamMelee[0].PositionHandler;
            lastMeleeHandler.isLastMelee = true;
            lastMeleeHandler.UpdateCanReposition();
        }
        else
        {
            for (int i = 0; i < teamMelee.Length; i++)
            {
                CharacterPositionHandler charHandler = teamMelee[i].PositionHandler;
                if (charHandler.isLastMelee)
                {
                    charHandler.isLastMelee = false;
                    charHandler.UpdateCanReposition();
                }
            }
        }
    }

    void RegisterCanRepositionListeners()
    {
        owner.ListenToAvailable(UpdateCanReposition);
        owner.ActionPointHandler.ListenToPoints(UpdateCanReposition);
    }

    void UpdateCanReposition()
    {
        bool result = !isLastMelee && preventingReposition == 0 &&
            owner.Available && owner.ActionPointHandler.HavePoints(costToReposition);
        if (result != canReposition.Value)
        {
            canReposition.Value = result;
        }
    }
}

public class TeamPositionVariable : ObservedVariable<TeamPosition> { }