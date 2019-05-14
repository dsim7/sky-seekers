using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class HelperMethods
{
    public static void CyclicalIncrement(ref int index, int max)
    {
        if (++index >= max)
        {
            index = 0;
        }
    }

    public static void CyclicalDecrement(ref int index, int max)
    {
        if (--index < 0)
        {
            index = max - 1;
        }
    }

    public static void UpdateTimer(ref float timer, float timerEnd, UnityAction action)
    {
        timer += Time.deltaTime;
        while (timer > timerEnd)
        {
            timer -= timerEnd;
            action.Invoke();
        }
    }

    public static void UpdateTimerUnscaled(ref float timer, float timerEnd, UnityAction action)
    {
        timer += Time.unscaledDeltaTime;
        while (timer > timerEnd)
        {
            timer -= timerEnd;
            action.Invoke();
        }
    }

    public static T Raycast<T>(Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            return hit.collider.GetComponent<T>();
        }
        return default(T);
    }

    public static bool CheckChance(float threshold)
    {
        return Random.Range(0, 1f) <= threshold;
    }
}
