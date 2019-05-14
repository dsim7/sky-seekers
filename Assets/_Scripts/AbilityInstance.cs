using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityInstance
{
    Ability ability;
    Character caster;
    List<Character> targets;
    UnityEvent onStart;
    UnityEvent onComplete;
    UnityEvent onCancel;
    bool isActive;

    public Ability Ability => ability;
    public Character Caster => caster;
    public List<Character> Targets => targets;
    
    static List<AbilityInstance> pool;
    static int poolIndex;

    static AbilityInstance()
    {
        pool = new List<AbilityInstance>();
        for (int i = 0; i < 10; i++)
        {
            pool.Add(new AbilityInstance());
        }
        poolIndex = 0;
    }

    static AbilityInstance GetNextInstance()
    {
        int startingIndex = poolIndex;
        AbilityInstance result = pool[poolIndex];
        while (result.isActive)
        {
            HelperMethods.CyclicalIncrement(ref poolIndex, pool.Count);
            result = pool[poolIndex];

            if (poolIndex == startingIndex)
            {
                Debug.LogWarning("No inactive instances available in object pool.");
                return null;
            }
        }
        HelperMethods.CyclicalIncrement(ref poolIndex, pool.Count);
        return result;
    }

    public static AbilityInstance NewAbility(Ability ability = null, Character caster = null)
    {
        AbilityInstance instance = GetNextInstance();
        instance.ability = ability;
        instance.caster = caster;
        instance.targets.Clear();
        instance.onStart.RemoveAllListeners();
        instance.onComplete.RemoveAllListeners();
        instance.onCancel.RemoveAllListeners();
        instance.isActive = true;
        return instance;
    }

    AbilityInstance(Ability ability = null, Character caster = null)
    {
        this.ability = ability;
        this.caster = caster;
        targets = new List<Character>();
        onStart = new UnityEvent();
        onComplete = new UnityEvent();
        onCancel = new UnityEvent();
    }

    public bool CheckTargets()
    {
        return targets.Count >= ability.Template.Targeting.NumTargetsRequired;
    }

    public void RegisterOnStartListener(UnityAction onStart)
    {
        this.onStart.AddListener(onStart);
    }

    public void RegisterOnCompleteListener(UnityAction onComplete)
    {
        this.onComplete.AddListener(onComplete);
    }

    public void RegisterOnCancelListener(UnityAction onCancel)
    {
        this.onCancel.AddListener(onCancel);
    }
    
    public void Execute()
    {
        onStart.Invoke();
        ability.Cooldown.StartCD();
        caster.Actor.StartCoroutine(DoActions());
    }

    public void Cancel()
    {
        onCancel.Invoke();
        isActive = false;
    }
    
    IEnumerator DoActions()
    {
        yield return null;
        foreach (AbilityAction action in ability.Template.Actions)
        {
            action.DoAction(caster, targets.ToArray());
            yield return new WaitForSeconds(action.DelayToNextAction);
        }
            
        onComplete.Invoke();
        isActive = false;
    }
}
