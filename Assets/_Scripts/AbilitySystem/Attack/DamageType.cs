using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DamageType : ScriptableObject
{
    [SerializeField]
    Color color;

    public Color Color => color;
}