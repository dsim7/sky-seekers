using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Team : ObservedMonoBehaviour
{
    [SerializeField]
    CombatController combatController;

    [SerializeField]
    TeamAnchors teamAnchors;

    [SerializeField]
    GameObject[] characterGameObjects;

    [SerializeField]
    TeamTemplate teamTemplate;
    public TeamTemplate TeamTemplate => teamTemplate;

    [SerializeField]
    Team enemyTeam;
    public Team EnemyTeam => enemyTeam;

    [SerializeField]
    TargetingController targetingController;
    public TargetingController TargetingController => targetingController;

    public List<Character> Characters { get; } = new List<Character>();
    public List<Character> MeleeCharacters { get; } = new List<Character>();
    public List<Character> SupportCharacters { get; } = new List<Character>();
    public TeamAIHandler AIHandler { get; private set; }
    
    float aiActionsDelayTimer;
    float aiDelay = 1f;
    UnityAction nextAIAction;

    public bool IsActive { get; private set; }

    void Awake()
    {
        InitializeCharacters();
        RepositionAnchors();

        AIHandler = new TeamAIHandler(this);
        AIHandler.AIFinishedEvent.AddListener(FinishTurn);
    }

    public void StartTurn()
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].StartTurn();
        }
        AIHandler.StartAIActions();

        IsActive = true;
        Changed();
    }

    void FinishTurn()
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].FinishTurn();
        }
        IsActive = false;
        Changed();

        combatController.RotateTurns();
    }

    public void UpdateCharacterPositionListing(Character character)
    {
        if (character.PositionHandler.Position == TeamPosition.Melee)
        {
            SupportCharacters.Remove(character);
            MeleeCharacters.Add(character);
        }
        else
        {
            MeleeCharacters.Remove(character);
            SupportCharacters.Add(character);
        }
    }
    
    void RepositionAnchors()
    {
        teamAnchors.AssignAnchors(this);
    }

    void InitializeCharacters()
    {
        for (int i = 0; i < characterGameObjects.Length; i++)
        {
            Characters.Add(new Character(teamTemplate.CharacterSlots[i],
                this,
                characterGameObjects[i].GetComponent<CharacterActor>(),
                characterGameObjects[i].GetComponent<CharacterMover>(),
                characterGameObjects[i].GetComponent<CharacterFloatingTextHandler>()));

            Characters[i].PositionHandler.ListenToPosition(() =>
            {
                RepositionAnchors();
            });

            Characters[i].PositionHandler.OnReposition.AddListener(UpdateCharacterPositionListing);

            List<Character> listToAddTo = Characters[i].PositionHandler.Position == TeamPosition.Melee ? MeleeCharacters : SupportCharacters;
            listToAddTo.Add(Characters[i]);
        }
    }
}

public enum TeamPosition
{
    Melee, Support
}
