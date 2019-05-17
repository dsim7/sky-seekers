using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInstance
{
    public StatusEffectTemplate Template { get; private set; }
    public Character Caster { get; private set; }
    public Character Target { get; private set; }
    public StatusEffectData Data { get; private set; }
    public int CurrentDuration { get; private set; }
    public int CurrentTickTime { get; private set; }

    List<ModifierBase> modifierList;
    List<SpecialEffect> persistentSfxList;

    public void OnApply(bool applyMods = true)
    {
        isActive = true;
        CurrentDuration = Template.Duration;
        CurrentTickTime = Template.TickRate;

        DoEffects(Template.OnApply);
        DoSFXs(Template.OnApplySFX);

        if (applyMods)
        {
            foreach (ModifierGenerator mod in Template.Modifiers)
            {
                modifierList.Add(mod.GenerateInstance());
            }

            foreach (SpecialEffectGenerator sfxRef in Template.PersistentSFX)
            {
                SpecialEffect sfx = sfxRef.Value.GenerateSFX(Target.Actor.transform);
                persistentSfxList.Add(sfx);
            }

            ManageModifiers(true);
        }
    }

    public void PerTurn()
    {
        CurrentTickTime--;
        if (Template.TickRate != 0 && CurrentTickTime <= 0)
        {
            DoEffects(Template.OnTick);
            DoSFXs(Template.OnTickSFX);
            CurrentTickTime = Template.TickRate;
        }
        CurrentDuration--;
    }

    public void OnRemove()
    {
        isActive = false;

        DoEffects(Template.OnRemove);
        DoSFXs(Template.OnRemoveSFX);
        ManageModifiers(false);
        
        foreach (SpecialEffect sfx in persistentSfxList)
        {
            sfx.Stop();
        }
    }

    public void Reset()
    {
        OnApply(false);
    }

    void DoEffects(AbilityOnHitEffect[] effectList)
    {
        foreach (AbilityOnHitEffect effect in effectList)
        {
            effect.TakeEffect(Caster, Target);
        }
    }

    void ManageModifiers(bool applyOrRemove)
    {
        foreach (ModifierBase mod in modifierList)
        {
            if (applyOrRemove)
            {
                mod.Apply(Target, this);
            }
            else
            {
                mod.Remove();
            }
        }
    }

    void DoSFXs(SpecialEffectGenerator[] sfxList)
    {
        foreach (SpecialEffectGenerator sfxRef in sfxList)
        {
            SpecialEffect sfx = sfxRef.Value.GenerateSFX(Target.Actor.transform);
        }
    }

    public class StatusEffectData
    {
        Dictionary<string, float> floatData = new Dictionary<string, float>();
        
        public float GetFloat(string name)
        {
            return floatData.ContainsKey(name) ? floatData[name] : float.NaN;
        }

        public void Reset()
        {
            floatData.Clear();
        }
    }

    #region Statics
    bool isActive;

    static List<StatusEffectInstance> pool;
    static int poolIndex;

    static StatusEffectInstance()
    {
        pool = new List<StatusEffectInstance>();
        for (int i = 0; i < 32; i++)
        {
            pool.Add(new StatusEffectInstance());
        }
        poolIndex = 0;
    }

    static StatusEffectInstance GetNextInstance()
    {
        int startingIndex = poolIndex;
        StatusEffectInstance result = pool[poolIndex];
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

    public static StatusEffectInstance NewStatus(StatusEffectTemplate template = null, Character caster = null, Character target = null)
    {
        StatusEffectInstance instance = GetNextInstance();
        instance.Template = template;
        instance.Caster = caster;
        instance.Target = target;
        instance.Data.Reset();
        instance.modifierList.Clear();
        instance.persistentSfxList.Clear();
        return instance;
    }

    StatusEffectInstance()
    {
        Data = new StatusEffectData();
        modifierList = new List<ModifierBase>();
        persistentSfxList = new List<SpecialEffect>();
    }
    #endregion
}
