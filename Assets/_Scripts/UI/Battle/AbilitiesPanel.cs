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
    AbilityType[] meleeAbilityTypes;
    [SerializeField]
    AbilityType[] supportAbilityTypes;

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
        AbilityType[] positionAbilityTypes = caster.PositionHandler.Position == TeamPosition.Melee ? meleeAbilityTypes : supportAbilityTypes;
        caster.AbilityHandler.CastAbility(positionAbilityTypes[index]);
    }

    public void SetCaster(CharacterActor casterActor)
    {
        caster = casterActor.Owner;
        UpdateAbilities();
    }

    public void UpdateAbilities()
    {
        AbilityType[] positionAbilityTypes = caster.PositionHandler.Position == TeamPosition.Melee ? meleeAbilityTypes : supportAbilityTypes;
        for (int i = 0; i < positionAbilityTypes.Length; i++)
        {
            if (buttons[i] != null && positionAbilityTypes[i] != null)
            {
                buttons[i].Ability = caster.AbilityHandler.GetAbility(positionAbilityTypes[i]);
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
