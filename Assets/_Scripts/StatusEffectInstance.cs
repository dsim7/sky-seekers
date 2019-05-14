using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInstance
{
    StatusEffectTemplate template;
    Character caster;
    Character target;
    List<SpecialEffect> persistentSfxList;
    int currentDuration;
    int currentTickTime;
    bool isActive;

    public StatusEffectTemplate Template => template;
    public Character Caster => caster;
    public Character Target => target;
    public int CurrentDuration => currentDuration;

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
        instance.template = template;
        instance.caster = caster;
        instance.target = target;
        return instance;
    }

    StatusEffectInstance(StatusEffectTemplate template = null, Character caster = null, Character target = null)
    {
        this.template = template;
        this.caster = caster;
        this.target = target;

        persistentSfxList = new List<SpecialEffect>();
    }

    public void OnApply(bool applyPassives = true)
    {
        isActive = true;
        currentDuration = template.Duration;
        currentTickTime = template.TickRate;

        DoEffects(template.OnApply);
        DoSFXs(template.OnApplySFX);

        if (applyPassives)
        {
            ApplyPassiveEffects(template.PassiveEffects, true);
        }

        foreach (SpecialEffectPoolRef sfxRef in template.PersistentSFX)
        {
            SpecialEffect sfx = sfxRef.Value.GenerateSFX(target.Actor.transform);
            persistentSfxList.Add(sfx);
        }
    }

    public void PerTurn()
    {
        currentTickTime--;
        if (template.TickRate != 0 && currentTickTime <= 0)
        {
            DoEffects(template.OnTick);
            DoSFXs(template.OnTickSFX);
            currentTickTime = template.TickRate;
        }
        currentDuration--;
    }

    public void OnRemove()
    {
        isActive = false;

        DoEffects(template.OnRemove);
        DoSFXs(template.OnRemoveSFX);
        ApplyPassiveEffects(template.PassiveEffects, false);

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
            effect.TakeEffect(caster, target);
        }
    }

    void ApplyPassiveEffects(PassiveEffect[] passivesList, bool applyOrRemove)
    {
        foreach (PassiveEffect passive in passivesList)
        {
            if (applyOrRemove)
            {
                passive.Apply(Target);
            }
            else
            {
                passive.Remove(Target);
            }
        }
    }

    void DoSFXs(SpecialEffectPoolRef[] sfxList)
    {
        foreach (SpecialEffectPoolRef sfxRef in sfxList)
        {
            SpecialEffect sfx = sfxRef.Value.GenerateSFX(target.Actor.transform);
        }
        persistentSfxList.Clear();
    }
}
