using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TeamTemplate : ScriptableObject
{
    [SerializeField]
    CharacterTemplateSlot[] characterSlots;
    public CharacterTemplateSlot[] CharacterSlots => characterSlots;

    [SerializeField]
    bool isAIControlled;
    public bool IsAIControlled => isAIControlled;

}
