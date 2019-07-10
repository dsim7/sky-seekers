using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    public CharacterTemplate character;
    public CharacterTemplateVariableSO selectedCharacter;

    public void Selected()
    {
        selectedCharacter.Value = character;
    }
}
