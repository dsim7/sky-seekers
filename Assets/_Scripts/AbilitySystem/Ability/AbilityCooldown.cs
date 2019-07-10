using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityCooldown
{
    int defaultCD, currentCD;

    BoolVariable isOffCD = new BoolVariable();
    public bool IsOffCD => isOffCD.Value;
    public void ListenToCD(UnityAction listener) { isOffCD.RegisterPostchangeEvent(listener); }
    public void UnlistenToCD(UnityAction listener) { isOffCD.UnregisterPostchangeEvent(listener); }

    public AbilityCooldown(int defaultCD)
    {
        this.defaultCD = defaultCD;
        isOffCD.Value = true;
    }

    public void StartCD()
    {
        currentCD = defaultCD;
        if (currentCD != 0)
        {
            isOffCD.Value = false;
        }
    }

    public void TickCD()
    {
        if (!IsOffCD)
        {
            currentCD--;
            if (currentCD == 0)
            {
                isOffCD.Value = true;
            }
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
