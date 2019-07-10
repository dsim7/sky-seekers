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
    public CharacterFloatingTextHandler FloatingTextHandler { get; private set; }

    public Team Team { get; }

    public float CritChance { get; } = 0.2f;
    public float Power { get; } = 1;
    public float CritMultiplier { get; } = 2f;
    public float Defense { get; } = 1;

    int incapacitationCount;
    public bool Incapacitated => incapacitationCount > 0;
    public void Incapacitate() { incapacitationCount++; if (incapacitationCount == 1) { UpdateAvailability(); } }
    public void RemoveIncapacitation() { incapacitationCount--; if (incapacitationCount == 0) { UpdateAvailability(); } }

    bool active;

    BoolVariable available = new BoolVariable();
    public bool Available => available.Value;
    public void ListenToAvailable(UnityAction listener) { available.RegisterPostchangeEvent(listener); }
    public void UnlistenToAvailable(UnityAction listener) { available.UnregisterPostchangeEvent(listener); }

    public float Threat { get; set; }
    public float Threatened { get; set; }

    int untargetableCount = 0;
    public void MakeUntargetable() { untargetableCount++; if (untargetableCount == 1) untargetable.Value = true; }
    public void MakeTargetable() { untargetableCount--; if (untargetableCount == 0) untargetable.Value = false; }
    BoolVariable untargetable = new BoolVariable();
    public bool Targetable => !untargetable.Value;
    public void ListenToTargetable(UnityAction listener) { untargetable.RegisterPostchangeEvent(listener); }
    public void UnlistenToTargetable(UnityAction listener) { untargetable.UnregisterPostchangeEvent(listener); }

    public Character(CharacterTemplateSlot templateSlot, Team team, 
        CharacterActor actor, CharacterMover mover, CharacterFloatingTextHandler floatingTextHandler)
    {
        Template = templateSlot.Template;
        Actor = actor;
        Mover = mover;
        FloatingTextHandler = floatingTextHandler;
        actor.Owner = this;
        mover.Owner = this;
        floatingTextHandler.Owner = this;

        Team = team;

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