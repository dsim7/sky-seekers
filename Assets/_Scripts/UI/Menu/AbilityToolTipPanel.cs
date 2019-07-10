using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityToolTipPanel : MonoBehaviour
{
    public AbilityTemplateVariable observedAbility;
    public RectTransform panelTransform;
    public TMPro.TextMeshProUGUI abilityName;
    public TMPro.TextMeshProUGUI abilityDescription;
    
    private void Start()
    {
        observedAbility.RegisterPostchangeEvent(UpdateInfo);
        gameObject.SetActive(false);
    }
    
    private void UpdateInfo()
    {
        if (observedAbility.Value != null)
        {
            abilityName.text = observedAbility.Value.name;
            abilityDescription.text = observedAbility.Value.Description;
        }
    }
}
