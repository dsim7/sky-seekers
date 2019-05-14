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
        
        character.ActionHandler.RegisterListener(UpdateAPOrbs);
        UpdateAPOrbs();

        character.TeamHandler.RegisterListener(UpdatePosition);
        UpdatePosition();
    }

    public void Reposition()
    {
        actor.Character.TeamHandler.Reposition();
    }

    void UpdateAPOrbs()
    {
        for (int i = 0; i < apOrbs.Length; i++)
        {
            apOrbs[i].color = i < character.ActionHandler.Points ? filledIn : empty;
        }
    }
    
    void UpdateHealth()
    {
        float curHealth = character.HealthHandler.Health;
        float maxHealth = character.HealthHandler.MaxHealth;
        healthBar.fillAmount = curHealth / maxHealth;

        healthText.text = sb.Clear().Append(curHealth.ToString()).Append(" / ").Append(maxHealth).ToString();
    }

    void UpdatePosition()
    {
        positionText.text = character.TeamHandler.Position.ToString();

        repositionButton.interactable = character.TeamHandler.CanReposition;
    }

}
