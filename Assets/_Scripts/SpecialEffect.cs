using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    [SerializeField]
    bool persistent;

    ParticleSystem particle;
    bool isActive;

    public bool IsActive => isActive;
    
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        particle.Play();
        if (persistent)
        {
            isActive = true;
        }
    }

    public void Stop()
    {
        particle.Stop();
        if (persistent)
        {
            isActive = false;
        }
    }
}
