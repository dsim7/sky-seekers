using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TeamAnchors : MonoBehaviour
{
    [Space]
    [SerializeField]
    Transform[] M3_M, M2_M, M2_S, M1_M, M1_S;

    public void AssignAnchors(Team team)
    {
        int numMelee = 0;
        for (int i = 0; i < team.Characters.Length; i++)
        {
            if (team.Characters[i].TeamHandler.Position == TeamPosition.Melee)
                numMelee++;
        }

        Transform[] meleeAnchors = null;
        Transform[] supportAnchors = null;

        switch (numMelee)
        {
            case 1:
                meleeAnchors = M1_M;
                supportAnchors = M1_S;
                break;
            case 2:
                meleeAnchors = M2_M;
                supportAnchors = M2_S;
                break;
            case 3:
                meleeAnchors = M3_M;
                supportAnchors = null;
                break;
        }
        
        int indexOfMelee = 0;
        int indexOfSupport = 0;
        for (int i = 0; i < team.Characters.Length; i++)
        {
            if (team.Characters[i].TeamHandler.Position == TeamPosition.Melee)
            {
                team.Characters[i].Actor.Mover.Anchor = meleeAnchors[indexOfMelee++];
            }
            else
            {
                team.Characters[i].Actor.Mover.Anchor = supportAnchors[indexOfSupport++];
            }
        }
    }
}

