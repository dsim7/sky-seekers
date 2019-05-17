
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    void Update()
    {
        Timer.UpdateActiveTimers();
    }
}

public class Timer
{
    static int poolSize = 30;
    static Stack<Timer> inactiveTimers = new Stack<Timer>();
    static List<Timer> activeTimers = new List<Timer>();

    static Timer()
    {
        for (int i = 0; i < poolSize; i++)
        {
            inactiveTimers.Push(new Timer());
        }
    }

    public static void UpdateActiveTimers()
    {
        for (int i = 0; i < activeTimers.Count; i++)
        {
            Timer timer = activeTimers[i];
            timer.Update();
        }
    }

    public static void RunTimerOnce(float interval = 1, UnityAction onComplete = null)
    {
        Timer timer = inactiveTimers.Pop();
        timer.Interval = interval;
        timer.OnComplete = onComplete;
        timer.Looping = false;
        timer.time = 0;
        timer.IsRunning = true;
        timer.disposeOnComplete = true;

        activeTimers.Add(timer);
    }

    public static Timer GetInstance(float interval = 1, UnityAction onComplete = null, bool continuous = false)
    {
        Timer timer = inactiveTimers.Pop();
        timer.Interval = interval;
        timer.OnComplete = onComplete;
        timer.Looping = continuous;
        timer.time = 0;
        timer.IsRunning = false;
        timer.disposeOnComplete = false;

        activeTimers.Add(timer);
        return timer;
    }
    
    float time;
    float interval;
    bool disposeOnComplete;
    public float Interval { get { return interval; } set { interval = Mathf.Max(value, 0); time = 0; } }
    public bool IsRunning { get; private set; }
    public bool Looping { get; set; }
    public UnityAction OnComplete { get; set; }

    public void Update()
    {
        if (!IsRunning)
        {
            return;
        }

        time += Time.deltaTime;
        if (time > Interval)
        {
            if (Looping)
            {
                time -= Interval;
                OnComplete?.Invoke();
            }
            else
            {
                OnComplete?.Invoke();
                Stop();
                Reset();
                if (disposeOnComplete)
                {
                    Dispose();
                }
            }
        }
    }

    public void Start()
    {
        IsRunning = true;
    }

    public void Reset()
    {
        time = 0;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void DisposeWhenDone()
    {
        disposeOnComplete = true;
    }

    void Dispose()
    {
        IsRunning = false;
        activeTimers.Remove(this);
        inactiveTimers.Push(this);
    }
}
