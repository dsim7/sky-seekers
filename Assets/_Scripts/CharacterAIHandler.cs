
using UnityEngine;

public class CharacterAIHandler : IProlongedAction
{
    public const float AI_ACTION_DELAY = 0.3f;
    public const float AI_INTERVAL_DELAY = 0.5f;

    Character owner;
    public Character Character => owner;

    CharacterAI aiTemplate;
    public CharacterAI AITemplate => aiTemplate;

    bool isAI;
    public bool IsAI => isAI;

    OneTimeEvent onCompleteAction = new OneTimeEvent();
    OneTimeEvent onCompletePostAction = new OneTimeEvent();
    
    public CharacterAIHandler(Character character)
    {
        owner = character;
        aiTemplate = owner.Template.AI;
        isAI = owner.Team.TeamTemplate.IsAIControlled;
    }

    public void StartAction()
    {
        // Listen for when the character is available again
        owner.ListenToAvailable(Act);

        // Prompts the character to do something. After which they will become unavailable
        // while doing it, and then become available again
        if (!aiTemplate.ConsiderRepositioning(owner))
        {
            Act();
        }
    }

    void Act()
    {
        if (owner.Available)
        {
            Timer.RunTimerOnce(AI_ACTION_DELAY, () =>
            {
                if (!aiTemplate.ConsiderCastingAbility(owner))
                {
                    owner.UnlistenToAvailable(Act);
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
