using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AbilityInstance
{
    Ability ability;
    public Ability Ability => ability;

    Character caster;
    public Character Caster => caster;

    List<Character> targets;
    public List<Character> Targets => targets;

    UnityEvent onStart;
    public UnityEvent OnStart => onStart;

    UnityEvent onComplete;
    public UnityEvent OnComplete => onComplete;

    UnityEvent onCancel;
    public UnityEvent OnCancel => onCancel;
    
    public bool CheckTargets()
    {
        return targets.Count >= ability.Template.Targeting.NumTargetsRequired;
    }

    public void Execute()
    {
        onStart.Invoke();
        TriggerMods();
        ability.Cooldown.StartCD();
        
        List<AbilityActionInstance> actionInsts = GenerateActionInstances();

        ActionSeriesExecuter seriesExecuter = new ActionSeriesExecuter();
        seriesExecuter.DoSeriesOfDelayedActions(actionInsts, () => 
        {
            onComplete.Invoke();
            isActive = false;
        });
    }

    public void Cancel()
    {
        onCancel.Invoke();
        isActive = false;
    }

    void TriggerMods()
    {
        caster.EventHandler.CastAbility.Invoke(this);
        foreach (Character target in targets)
        {
            target.EventHandler.TargetedByAbility.Invoke(this);
        }
    }

    List<AbilityActionInstance> GenerateActionInstances()
    {
        List<AbilityActionInstance> actionInsts = new List<AbilityActionInstance>();
        foreach (AbilityAction action in ability.Template.Actions)
        {
            actionInsts.Add(new AbilityActionInstance(action, caster, targets));
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

    #endregion
}
