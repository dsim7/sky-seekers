using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ModifierBase
{
    public abstract void Apply(Character character, StatusEffectInstance origin = null);

    public abstract void Remove();
}

public abstract class Modifier<T> : ModifierBase
{
    public Character Character { get; private set; }
    public StatusEffectInstance OriginStatusEffect { get; private set; }

    protected abstract UnityEvent<T> GetEvent(CharacterEventHandler eventHandler);

    public override void Apply(Character character, StatusEffectInstance origin = null)
    {
        Character = character;
        OriginStatusEffect = origin;
        UnityEvent<T> evnt = GetEvent(character.EventHandler);
        evnt.AddListener(Modify);
    }

    public override void Remove()
    {
        UnityEvent<T> evnt = GetEvent(Character.EventHandler);
        evnt.RemoveListener(Modify);
    }

    protected abstract void Modify(T instance);
}

