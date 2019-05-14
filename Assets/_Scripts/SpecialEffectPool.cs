using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectPool : MonoBehaviour
{
    [SerializeField]
    SpecialEffectPoolRef reference;
    [SerializeField]
    SpecialEffect sfxPrefab;

    List<SpecialEffect> pool;
    int poolIndex;
    int poolSize = 10;

    void Start()
    {
        reference.Value = this;

        pool = new List<SpecialEffect>();
        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(Instantiate(sfxPrefab, transform));
        }
    }

    public SpecialEffect GenerateSFX(Transform attach)
    {
        SpecialEffect sfx = GetNext();
        if (sfx != null)
        {
            sfx.transform.position = attach.position;
            sfx.transform.SetParent(attach);
            sfx.Play();
        }
        return sfx;
    }

    SpecialEffect GetNext()
    {
        int startingIndex = poolIndex;
        SpecialEffect result = pool[poolIndex];

        while (result.IsActive)
        {
            HelperMethods.CyclicalIncrement(ref poolIndex, pool.Count);
            result = pool[poolIndex];

            if (poolIndex == startingIndex)
            {
                Debug.LogError("No inactive objects available in pool.");
                return null;
            }
        }

        HelperMethods.CyclicalIncrement(ref poolIndex, pool.Count);
        return result;
    }
}
