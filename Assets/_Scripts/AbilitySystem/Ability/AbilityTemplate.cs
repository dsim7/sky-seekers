using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class AbilityTemplate : ScriptableObject
{
    [SerializeField]
    AbilityAction[] actions;
    [SerializeField]
    AbilityTargeting targeting;
    [SerializeField]
    int cooldown;
    [SerializeField]
    AbilityType type;
    [SerializeField]
    int actionPointCost;

    [SerializeField]
    bool mustBeAbleToReposition;
    [SerializeField]
    AbilityFlag flags;

    [Space]
    [SerializeField]
    [TextArea]
    string description;

    public AbilityAction[] Actions => actions;
    public AbilityTargeting Targeting => targeting;
    public int Cooldown => cooldown;
    public AbilityType Type => type;
    public int ActionPointCost => actionPointCost;
    public AbilityFlag Flags => flags;
    public bool MustBeAbleToReposition => mustBeAbleToReposition;
    public string Description => description;
}
