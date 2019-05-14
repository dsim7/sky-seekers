using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum AbilityFlag
{
    None = 0,
    Friendly = 1,
    Offensive = 2,
    Damaging = 4,
    TargetSupports = 8,
    Control = 16,
    Defensive = 32
}