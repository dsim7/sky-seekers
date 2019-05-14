using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterActionHandler
{
    Character owner;

    IntVariable actionPoints;

    public int Points => actionPoints.Value;

    public CharacterActionHandler(Character character)
    {
        owner = character;
        actionPoints = new IntVariable();
    }

    public void ResetActionPoints()
    {
        actionPoints.Value = owner.Template.StartingActionPoints;
    }

    public void UseActionPoints(int amount)
    {
        actionPoints.Value = Mathf.Clamp(actionPoints.Value - amount, 0, int.MaxValue);
    }

    public bool HavePoints(int amount)
    {
        return actionPoints.Value >= amount;
    }

    public void RegisterListener(UnityAction action)
    {
        actionPoints.RegisterPostchangeEvent(action);
    }

    public void UnregisterListener(UnityAction action)
    {
        actionPoints.UnregisterPostchangeEvent(action);
    }
}
