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
        character = actor.Character;
        
        character.HealthHandler.RegisterListener(UpdateHealth);
        UpdateHealth();
        
        character.ActionPointHandler.ListenToPoints(UpdateAPOrbs);
        UpdateAPOrbs();

        character.PositionHandler.ListenToCanReposition(UpdateRepositionButton);
        UpdateRepositionButton();

        character.PositionHandler.ListenToPosition(UpdatePosition);
        UpdatePosition();
    }

    public void Reposition()
    {
        actor.Character.PositionHandler.Reposition();
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

        healthText.text = sb.Clear().Append(curHealth.ToString()).Append(" / ").Append(maxHealth).ToString();
    }

    void UpdateRepositionButton()
    {
        repositionButton.interactable = character.PositionHandler.CanReposition;
    }

    void UpdatePosition()
    {
        positionText.text = character.PositionHandler.Position.ToString();
    }

}
