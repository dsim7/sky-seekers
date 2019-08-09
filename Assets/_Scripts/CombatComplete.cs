using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatComplete : MonoBehaviour
{
    [SerializeField] GameObject victoryText, defeatText;
    [SerializeField] GameObject combatUI;
    [SerializeField] Team playerTeam, enemyTeam;

    private void Start()
    {
        RegisterListeners();
    }

    public void RegisterListeners()
    {
        playerTeam.Characters.ForEach(c => c.HealthHandler.RegisterListener(CheckTeamDead));
        enemyTeam.Characters.ForEach(c => c.HealthHandler.RegisterListener(CheckTeamDead));
    }

    void Victory()
    {
        StartCoroutine(VictorySequence());
    }

    void Defeat()
    {
        StartCoroutine(DefeatSequence());
    }

    IEnumerator VictorySequence()
    {
        victoryText.SetActive(true);
        defeatText.SetActive(false);
        combatUI.SetActive(false);
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene("Menu");
    }

    IEnumerator DefeatSequence()
    {
        defeatText.SetActive(true);
        victoryText.SetActive(false);
        combatUI.SetActive(false);
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene("Menu");
    }

    void CheckTeamDead()
    {
        if (playerTeam.Characters.TrueForAll(c => c.HealthHandler.Dead))
        {
            Defeat();
        }
        else if (enemyTeam.Characters.TrueForAll(c => c.HealthHandler.Dead))
        {
            Victory();
        }
    }
}
