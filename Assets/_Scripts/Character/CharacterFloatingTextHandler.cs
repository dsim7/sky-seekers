using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFloatingTextHandler : MonoBehaviour
{
    public Character Owner;
    public FloatingCombatTextPoolRef poolRef;
    
    public void ShowText(Transform transform, string text, Color color = default)
    {
        poolRef.Value.ShowText(transform, text, color);
    }
}
