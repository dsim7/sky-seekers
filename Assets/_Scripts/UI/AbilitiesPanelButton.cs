using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesPanelButton : MonoBehaviour
{
    Button button;

    [SerializeField]
    TextMeshProUGUI buttonText;

    Ability ability;
    public Ability Ability
    {
        get { return ability; }

        set
        {
            if (ability != null)
            {
                ability.UnlistenToCanCast(UpdateCanCast);
            }
            ability = value;
            if (ability != null)
            {
                ability.ListenToCanCast(UpdateCanCast);
                UpdateCanCast();
                buttonText.text = ability.Template.name;
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    void Start()
    {
        button = GetComponent<Button>();
    }

    void UpdateCanCast()
    {
        button.interactable = ability.CanCast;
    }

}
