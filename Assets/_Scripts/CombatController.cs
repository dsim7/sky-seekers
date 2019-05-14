using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    [SerializeField]
    Team[] teams;
    
    int currentTeamIndex;
    public int CurrentTeamIndex => currentTeamIndex;
    
    void Start()
    {
        currentTeamIndex = 0;
        teams[currentTeamIndex].Active = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SwitchTurns();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            teams[0].Characters[0].TeamHandler.Reposition();
        }
    }
    
    public void SwitchTurns()
    {
        teams[currentTeamIndex].Active = false;
        HelperMethods.CyclicalIncrement(ref currentTeamIndex, teams.Length);
        teams[currentTeamIndex].Active = true;
    }
}
