using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealthPanel : MonoBehaviour
{
    [SerializeField]
    CharacterActor characterActor;
    [SerializeField]
    Image healthBar;
    [SerializeField]
    TextMeshProUGUI healthText;

    CharacterHealthHandler health;
    StringBuilder sb;

    void Start()
    {
        sb = new StringBuilder();

        health = characterActor.Owner.HealthHandler;
        health.RegisterListener(UpdateInfo);
        UpdateInfo();
    }

    void UpdateInfo()
    {
        float curHealth = health.Health;
        float maxHealth = health.MaxHealth;
        healthBar.fillAmount = curHealth / maxHealth;

        healthText.text = sb.Clear().Append(curHealth.ToString("#")).Append(" / ").Append(maxHealth).ToString();
    }
}
