using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class PassiveEffect : ScriptableObject
{
    public abstract void Apply(Character character);

    public abstract void Remove(Character character);
}

public abstract class PassiveEffect<T> : PassiveEffect
{
    protected abstract UnityEvent<T> GetEvent(CharacterEventHandler eventHandler);

    public override void Apply(Character character)
    {
        UnityEvent<T> evnt = GetEvent(character.EventHandler);
        evnt.AddListener(Effect);
    }

    public override void Remove(Character character)
    {
        UnityEvent<T> evnt = GetEvent(character.EventHandler);
        evnt.RemoveListener(Effect);
    }

    protected abstract void Effect(T instance);
}

