using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Character
{
    public CharacterTemplate Template { get; private set; }
    public CharacterActor Actor { get; private set; }
    public CharacterMover Mover { get; private set; }
    public CharacterPositionHandler PositionHandler { get; private set; }
    public CharacterAbilityHandler AbilityHandler { get; private set; }
    public CharacterEventHandler EventHandler { get; private set; }
    public CharacterStatusHandler StatusHandler { get; private set; }
    public CharacterHealthHandler HealthHandler { get; private set; }
    public CharacterActionPointHandler ActionPointHandler { get; private set; }
    public CharacterAIHandler AIHandler { get; private set; }

    Team team;
    public Team Team => team;

    float power, critChance, critMultiplier, defense;
    public float CritChance => critChance;
    public float Power => power;
    public float CritMultiplier => critMultiplier;
    public float Defense => defense;
    
    int incapacitationCount;
    public bool Incapacitated => incapacitationCount > 0;
    bool active;

    BoolVariable available = new BoolVariable();
    public bool Available => available.Value;
    public void ListenToAvailable(UnityAction listener) { available.RegisterPostchangeEvent(listener); }
    public void UnlistenToAvailable(UnityAction listener) { available.UnregisterPostchangeEvent(listener); }

    float threat;
    public float Threat => threat;
    float threatened;
    public float Threatened => threatened;

    public Character(CharacterTemplateSlot templateSlot, Team team, CharacterActor actor, CharacterMover mover)
    {
        Debug.Log("making character");
        Template = templateSlot.Template;
        Actor = actor;
        Mover = mover;
        actor.Character = this;
        mover.Character = this;
        this.team = team;

        ActionPointHandler = new CharacterActionPointHandler(this);
        PositionHandler = new CharacterPositionHandler(this, templateSlot.Position);
        AbilityHandler = new CharacterAbilityHandler(this);
        EventHandler = new CharacterEventHandler(this);
        StatusHandler = new CharacterStatusHandler(this);
        HealthHandler = new CharacterHealthHandler(this);
        AIHandler = new CharacterAIHandler(this);

        RegisterAvailabilityListeners();
    }

    public void StartTurn()
    {
        ActionPointHandler.ResetActionPoints();
        StatusHandler.PerTurnStatuses();
        AbilityHandler.TickCooldowns();
        active = true;
        UpdateAvailability();
    }

    public void FinishTurn()
    {
        StatusHandler.RemoveExpiredStatuses();
        active = false;
        UpdateAvailability();
    }

    public void Incapacitate()
    {
        incapacitationCount++;
        if (incapacitationCount == 1)
        {
            UpdateAvailability();
        }
    }

    public void RemoveIncapacitation()
    {
        incapacitationCount--;
        if (incapacitationCount == 0)
        {
            UpdateAvailability();
        }
    }

    void RegisterAvailabilityListeners()
    {
        AbilityHandler.ListenToCasting(UpdateAvailability);
        PositionHandler.ListenToIsRepositioning(UpdateAvailability);
    }

    void UpdateAvailability()
    {
        bool result = !AbilityHandler.Casting && !PositionHandler.IsRepositioning && active && !Incapacitated;
        if (result != available.Value)
        {
            available.Value = result;
        }
    }
}

public class CharacterEvent : UnityEvent<Character> { }