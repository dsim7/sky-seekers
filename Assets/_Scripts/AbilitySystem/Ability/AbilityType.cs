using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AbilityType : ScriptableObject
{
    [SerializeField]
    TeamPosition positionReq;
    public TeamPosition PositionReq => positionReq;
}