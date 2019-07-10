
using UnityEngine;

public class CharacterAIHandler : IProlongedAction
{
    public const float AI_ACTION_DELAY = 0.3f;
    public const float AI_INTERVAL_DELAY = 0.5f;

    public Character Character { get; }
    public CharacterAI AITemplate { get; }
    public bool IsAI { get; }

    OneTimeEvent onCompleteAction = new OneTimeEvent();
    OneTimeEvent onCompletePostAction = new OneTimeEvent();
    
    public CharacterAIHandler(Character character)
    {
        Character = character;
        AITemplate = Character.Template.AI;
        IsAI = Character.Team.TeamTemplate.IsAIControlled;
    }

    public void StartAction()
    {
        if (!Character.Available)
        {
            onCompleteAction.Invoke();
            return;
        }

        // Listen for when the character is available again
        Character.ListenToAvailable(Act);

        // Prompts the character to do something. After which they will become unavailable
        // while doing it, and then become available again
        if (!AITemplate.ConsiderRepositioning(Character))
        {
            Act();
        }
    }

    void Act()
    {
        if (Character.Available)
        {
            Timer.RunTimerOnce(AI_ACTION_DELAY, () =>
            {
                if (!AITemplate.ConsiderCastingAbility(Character))
                {
                    Character.UnlistenToAvailable(Act);
                    onCompleteAction.Invoke();
                }
            });
        }
    }

    public OneTimeEvent GetActionCompleteEvent()
    {
        return onCompleteAction;
    }

    public float GetPostActionDelay()
    {
        return AI_INTERVAL_DELAY;
    }

    public OneTimeEvent GetActionPostCompleteEvent()
    {
        return onCompletePostAction;
    }
}
