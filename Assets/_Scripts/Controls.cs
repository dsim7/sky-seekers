using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField]
    Team playerTeam;
    [SerializeField]
    AbilityType mainAttack;

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    playerTeam.Characters[0].CastAbility(mainAttack);
        //}
    }
}
