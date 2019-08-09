
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
        teams[currentTeamIndex].StartTurn();
    }
    
    public void RotateTurns()
    {
        HelperMethods.CyclicalIncrement(ref currentTeamIndex, teams.Length);
        teams[currentTeamIndex].StartTurn();
    }
}
