using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterTemplateSlot
{
    [SerializeField]
    CharacterTemplate template;
    public CharacterTemplate Template => template;

    [SerializeField]
    TeamPosition position;
    public TeamPosition Position => position;

}
