using System;
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

    public static void UpdateTimerOnce(ref float timer, float timerEnd, UnityAction action)
    {
        timer += Time.deltaTime;
        if (timer >= timerEnd)
        {
            timer = 0;
            action.Invoke();
        }
    }

    public static void UpdateTimerRepeat(ref float timer, float timerEnd, UnityAction action)
    {
        timer += Time.deltaTime;
        while (timer > timerEnd)
        {
            timer -= timerEnd;
            action.Invoke();
        }
    }

    public static void UpdateTimerRepeatUnscaled(ref float timer, float timerEnd, UnityAction action)
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
        return UnityEngine.Random.Range(0, 1f) <= threshold;
    }

    public static void WaitUntil(OneTimeEvent evnt, UnityAction action)
    {
        evnt.AddListener(action);
    }

    public static void WaitThen(float howLong, UnityAction action)
    {
        Timer.GetInstance(howLong, action, false).Start();
    }

    public static Timer Loop(float interval, UnityAction action)
    {
        Timer result = Timer.GetInstance(interval, action, true);
        result.Start();
        return result;
    }
}


public class ActionSeriesExecuter
{
    Timer timer;

    public void DoSeriesOfDelayedActions<T>(List<T> actions, UnityAction onComplete) where T : IProlongedAction
    {
        timer = Timer.GetInstance();
        timer.Looping = true;

        for (int i = 0; i < actions.Count; i++)
        {
            T action = actions[i];
            T nextAction = i + 1 < actions.Count ? actions[i + 1] : default(T);
            action.GetActionCompleteEvent().AddListener(() =>
            {
                timer.Interval = action.GetPostActionDelay();
                
                if (nextAction != null)
                {
                    timer.OnComplete = () =>
                    {
                        timer.Stop();
                        action.GetActionPostCompleteEvent().Invoke();
                        nextAction.StartAction();
                    };
                }
                else
                {
                    timer.Looping = false;
                    timer.DisposeWhenDone();
                    timer.OnComplete = () =>
                    {
                        action.GetActionPostCompleteEvent().Invoke();
                        onComplete.Invoke();
                    };
                }

                timer.Start();
            });
        }
        
        actions[0].StartAction();
    }
}

public interface IProlongedAction
{
    void StartAction();

    OneTimeEvent GetActionCompleteEvent();

    float GetPostActionDelay();

    OneTimeEvent GetActionPostCompleteEvent();
}

public class OneTimeEvent
{
    UnityEvent evnt = new UnityEvent();

    public void AddListener(UnityAction listener)
    {
        evnt.AddListener(listener);
    }

    public void RemoveListener(UnityAction listener)
    {
        evnt.AddListener(listener);
    }

    public void Invoke()
    {
        evnt.Invoke();
        evnt.RemoveAllListeners();
    }
}
