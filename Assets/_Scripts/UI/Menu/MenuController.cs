using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject characterInfoPanel;
    public AbilityTemplateVariable selectedAbility;
    public GameObject tooltipPanel;

    public void ToggleCharacterInfoPanel()
    {
        characterInfoPanel.SetActive(!characterInfoPanel.activeSelf);
    }

    public void GoToBattle()
    {
        SceneManager.LoadScene("Game");
    }

    public void ToggleAbilityTooltip()
    {
        if (selectedAbility.Value != null)
        {
            tooltipPanel.SetActive(true);
        }
        else
        {
            tooltipPanel.SetActive(false);
        }
    }
}
