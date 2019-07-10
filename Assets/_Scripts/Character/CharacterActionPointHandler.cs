using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterActionPointHandler
{
    Character owner;
    public Character Character => owner;

    IntVariable actionPoints = new IntVariable();
    public int Points => actionPoints.Value;
    public void ListenToPoints(UnityAction listener) { actionPoints.RegisterPostchangeEvent(listener); }
    public void UnlistenToPoints(UnityAction listener) { actionPoints.UnregisterPostchangeEvent(listener); }

    public CharacterActionPointHandler(Character character)
    {
        owner = character;
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

}
