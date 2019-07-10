using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AbilityInstance
{
    public Ability Ability { get; private set; }
    public Character Caster { get; private set; }
    public List<Character> Targets { get; }
    public UnityEvent OnStart { get; }
    public UnityEvent OnComplete { get; }
    public UnityEvent OnCancel { get; }

    public bool CheckTargets()
    {
        return Targets.Count >= Ability.Template.Targeting.NumTargetsRequired;
    }

    public void Execute()
    {
        OnStart.Invoke();
        TriggerMods();
        Ability.Cooldown.StartCD();
        
        List<AbilityActionInstance> actionInsts = GenerateActionInstances();

        ActionSeriesExecuter seriesExecuter = new ActionSeriesExecuter();
        seriesExecuter.DoSeriesOfDelayedActions(actionInsts, () => 
        {
            OnComplete.Invoke();
            isActive = false;
        });
    }

    public void Cancel()
    {
        OnCancel.Invoke();
        isActive = false;
    }

    void TriggerMods()
    {
        Caster.EventHandler.CastAbility.Invoke(this);
        foreach (Character target in Targets)
        {
            target.EventHandler.TargetedByAbility.Invoke(this);
        }
    }

    List<AbilityActionInstance> GenerateActionInstances()
    {
        List<AbilityActionInstance> actionInsts = new List<AbilityActionInstance>();
        foreach (AbilityAction action in Ability.Template.Actions)
        {
            actionInsts.Add(new AbilityActionInstance(action, Caster, Targets));
        }
        return actionInsts;
    }

    #region Statics
    bool isActive;

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
        instance.Ability = ability;
        instance.Caster = caster;
        instance.Targets.Clear();
        instance.OnStart.RemoveAllListeners();
        instance.OnComplete.RemoveAllListeners();
        instance.OnCancel.RemoveAllListeners();
        instance.isActive = true;
        return instance;
    }

    AbilityInstance(Ability ability = null, Character caster = null)
    {
        Ability = ability;
        Caster = caster;
        Targets = new List<Character>();
        OnStart = new UnityEvent();
        OnComplete = new UnityEvent();
        OnCancel = new UnityEvent();
    }

    #endregion
}
