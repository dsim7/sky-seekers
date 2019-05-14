using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesPanelButton : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI buttonText;

    Ability ability;
    public Ability Ability
    {
        get { return ability; }

        set
        {
            ability = value;
            if (ability != null)
            {
                gameObject.SetActive(true);
                buttonText.text = ability.Template.name;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

}
