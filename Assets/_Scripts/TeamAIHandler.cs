
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TeamAIHandler
{
    Team team;
    public Team Team => team;

    bool isAIControlled;
    public bool IsAIControlled => isAIControlled;
    
    UnityEvent aiFinished = new UnityEvent();
    public UnityEvent AIFinishedEvent => aiFinished;

    public TeamAIHandler(Team team)
    {
        this.team = team;
        isAIControlled = team.TeamTemplate.IsAIControlled;
    }

    public void StartAIActions()
    {
        if (isAIControlled)
        {
            List<CharacterAIHandler> aiPrioritized = team.Characters.Select(c => c.AIHandler).
                OrderByDescending(ai => ai.AITemplate.TurnPriority).ToList();

            ActionSeriesExecuter seriesExecuter = new ActionSeriesExecuter();
            seriesExecuter.DoSeriesOfDelayedActions(aiPrioritized, aiFinished.Invoke);
        }
    }
}

