using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCooldown
{
    int defaultCD, currentCD;

    public bool IsOffCD => currentCD == 0;

    public AbilityCooldown(int defaultCD)
    {
        this.defaultCD = defaultCD;
    }

    public void StartCD()
    {
        currentCD = defaultCD;
    }

    public void TickCD()
    {
        if (!IsOffCD)
        {
            currentCD--;
        }
    }

    public void IncreaseCD(int amount)
    {
        if (!IsOffCD)
        {
            currentCD += amount;
        }
    }
}
