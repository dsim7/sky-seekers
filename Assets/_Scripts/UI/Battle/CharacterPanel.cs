using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField]
    CharacterActor actor;
    [Space]
    [SerializeField]
    Image[] apOrbs;
    [SerializeField]
    Color filledIn, empty;
    [Space]
    [SerializeField]
    Image healthBar;
    [SerializeField]
    TextMeshProUGUI healthText;
    [Space]
    [SerializeField]
    Button repositionButton;
    [SerializeField]
    TextMeshProUGUI positionText;

    Character character;
    StringBuilder sb;

    public CharacterActor Actor => actor;

    void Start()
    {
        sb = new StringBuilder();
        character = actor.Owner;
        
        character.HealthHandler.RegisterListener(UpdateHealth);
        UpdateHealth();
        
        character.ActionPointHandler.ListenToPoints(UpdateAPOrbs);
        UpdateAPOrbs();

        character.PositionHandler.ListenToCanDoRepositionAction(UpdateRepositionButton);
        UpdateRepositionButton();

        character.PositionHandler.ListenToPosition(UpdatePosition);
        UpdatePosition();
    }

    public void Reposition()
    {
        actor.Owner.PositionHandler.DoRepositionAction();
    }

    void UpdateAPOrbs()
    {
        for (int i = 0; i < apOrbs.Length; i++)
        {
            apOrbs[i].color = i < character.ActionPointHandler.Points ? filledIn : empty;
        }
    }
    
    void UpdateHealth()
    {
        float curHealth = character.HealthHandler.Health;
        float maxHealth = character.HealthHandler.MaxHealth;
        healthBar.fillAmount = curHealth / maxHealth;

        healthText.text = sb.Clear().Append(curHealth.ToString("0")).Append(" / ").Append(maxHealth.ToString("0")).ToString();
    }

    void UpdateRepositionButton()
    {
        repositionButton.interactable = character.PositionHandler.CanDoRepositionAction;
    }

    void UpdatePosition()
    {
        positionText.text = character.PositionHandler.Position.ToString();
    }

}
