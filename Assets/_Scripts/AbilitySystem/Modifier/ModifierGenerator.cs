using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ModifierGenerator : ScriptableObject
{
    public abstract ModifierBase GenerateInstance();
}

