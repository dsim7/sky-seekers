using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour
{
    [HideInInspector]
    public AbilityTemplate ability;
    public AbilityTemplateVariable selectedAbility;

    public void Selected()
    {
        selectedAbility.Value = ability;
    }
}
