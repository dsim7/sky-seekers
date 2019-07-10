using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCombatTextPool : MonoBehaviour
{
    public FloatingCombatTextPoolRef reference;
    public List<FloatingCombatText> pool;
    public FloatingCombatText fctPrefab;
    
    private void Start()
    {
        reference.Value = this;
    }

    public void ShowText(Transform anchor, string text, Color color = default)
    {
        FloatingCombatText nextFCT = GetNextFCT();
        nextFCT.Appear(anchor, text, color);
    }

    private FloatingCombatText GetNextFCT()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].active)
            {
                return pool[i];
            }
        }
        FloatingCombatText newFCT = Instantiate(fctPrefab, transform);
        pool.Add(newFCT);
        return newFCT;
    }
}
