using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    [SerializeField]
    TeamTemplate teamTemplate;

    [SerializeField]
    TeamAnchors teamAnchors;

    [SerializeField]
    CharacterActor[] actors;

    [SerializeField]
    Team enemyTeam;
    public Team EnemyTeam => enemyTeam;

    [SerializeField]
    TeamAI ai;
    public TeamAI AI => ai;

    [SerializeField]
    TargetingController targetingController;
    public TargetingController TargetingController => targetingController;

    Character[] characters;
    public Character[] Characters => characters;
    
    bool active;
    public bool Active
    {
        get { return active; }

        set
        {
            active = value;
            if (value)
            {
                StartTurn();
                if (AI != null)
                {
                    // Get AI Action
                }
            }
            else
            {
                FinishTurn();
            }
        }
    }

    void Awake()
    {
        characters = new Character[actors.Length];
        for (int i = 0; i < actors.Length; i++)
        {
            characters[i] = new Character(teamTemplate.CharacterSlots[i].Template,
                actors[i], this, teamTemplate.CharacterSlots[i].Position);
        }
        RepositionAnchors();
    }

    public void StartTurn()
    {
        foreach (Character character in characters)
        {
            character.StartTurn();
        }
    }

    public void FinishTurn()
    {
        foreach (Character character in characters)
        {
            character.FinishTurn();
        }
    }

    public void RepositionAnchors()
    {
        teamAnchors.AssignAnchors(this);
    }
}

public enum TeamPosition
{
    Melee, Support
}
