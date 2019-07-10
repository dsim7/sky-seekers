using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoPanel : MonoBehaviour
{
    public CharacterTemplateVariableSO selectedCharacter;

    public AbilityButton[] abilityButtons;
    public TMPro.TextMeshProUGUI nameDisplay;
    public TMPro.TextMeshProUGUI healthDisplay;
    public TMPro.TextMeshProUGUI powerDisplay;
    public TMPro.TextMeshProUGUI defenseDisplay;

    private void Start()
    {
        selectedCharacter.RegisterPostchangeEvent(UpdateInfo);
        for (int i = 0; i < abilityButtons.Length && i < selectedCharacter.Value.Abilities.Count; i++)
        {
            abilityButtons[i].ability = selectedCharacter.Value.Abilities[i];
        }

        gameObject.SetActive(false);
    }
    
    private void UpdateInfo()
    {
        if (selectedCharacter.Value != null)
        {
            gameObject.SetActive(true);
            nameDisplay.text = selectedCharacter.Value.name;
            healthDisplay.text = selectedCharacter.Value.MaxHealth.ToString();
            powerDisplay.text = selectedCharacter.Value.Attack.ToString();
            defenseDisplay.text = selectedCharacter.Value.Defense.ToString();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
