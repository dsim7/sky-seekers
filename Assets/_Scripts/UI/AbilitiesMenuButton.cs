using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesMenuButton : MonoBehaviour
{
    [SerializeField]
    CharacterPanel characterPanel;
    [SerializeField]
    AbilitiesPanel abilitiesPanel;

    public void OpenAbilitiesMenu()
    {
        if (!abilitiesPanel.gameObject.activeSelf)
        {
            abilitiesPanel.SetCaster(characterPanel.Actor);
            abilitiesPanel.transform.position = transform.position;
            abilitiesPanel.gameObject.SetActive(true);
        }
        else
        {
            abilitiesPanel.gameObject.SetActive(false);
        }
    }
}
