using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInstance
{
    public Character Dealer;
    public Character Target;
    public float Amount;
    public float Modifier;
    public float CritChanceOverride;
    public float CritMultiplierOverride;
    public bool IsCrit;
    public bool IsDefended;
    public bool IsMiss;
    public bool CannotCrit;
    public bool CannotBeDefended;
    public bool CannotMiss;
    public AttackType AttackType;
    public DamageType DamageType;
    
    public void DoDamage()
    {
        Dealer.HealthHandler.DealDamage(this);
        isActive = false;
    }

    #region Statics
    bool isActive = false;

    static List<DamageInstance> pool;
    static int poolIndex;

    static DamageInstance()
    {
        pool = new List<DamageInstance>();
        for (int i = 0; i < 10; i++)
        {
            pool.Add(new DamageInstance());
        }
        poolIndex = 0;
    }

    static DamageInstance GetNextInstance()
    {
        int startingIndex = poolIndex;
        DamageInstance result = pool[poolIndex];
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

    public static DamageInstance NewAttack(Character dealer = null, Character target = null, float damage = 0,
        DamageType damageType = null, AttackType attackType = null, float critChanceOverride = -1f, float critMultiplierOverride = -1f,
        float modifier = 1)
    {
        DamageInstance instance = GetNextInstance();
        instance.Dealer = dealer;
        instance.Target = target;
        instance.Amount = damage;
        instance.Modifier = modifier;
        instance.CritChanceOverride = critChanceOverride;
        instance.CritMultiplierOverride = critMultiplierOverride;
        instance.AttackType = attackType;
        instance.DamageType = damageType;
        instance.IsCrit = false;
        instance.IsDefended = false;
        instance.IsMiss = false;
        instance.CannotCrit = false;
        instance.CannotBeDefended = false;
        instance.CannotMiss = false;

        instance.isActive = true;
        return instance;
    }

    DamageInstance() { }
    #endregion
}
