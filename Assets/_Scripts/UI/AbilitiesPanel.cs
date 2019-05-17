using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitiesPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    AbilitiesPanelButton[] buttons;
    [SerializeField]
    AbilityType[] abilityTypes;

    Character caster;
    bool pointerInObject;
    
    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !pointerInObject)
        {
            gameObject.SetActive(false);
        }
    }

    public void CastAbility(int index)
    {
        caster.AbilityHandler.CastAbility(abilityTypes[index]);
    }

    public void SetCaster(CharacterActor casterActor)
    {
        caster = casterActor.Character;
        UpdateAbilities();
    }

    public void UpdateAbilities()
    {
        for (int i = 0; i < abilityTypes.Length; i++)
        {
            if (buttons[i] != null && abilityTypes[i] != null)
            {
                buttons[i].Ability = caster.AbilityHandler.GetAbility(abilityTypes[i]);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerInObject = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerInObject = false;
    }
}
