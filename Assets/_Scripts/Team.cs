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

    Character[] characters;
    public Character[] Characters => characters;

    List<Character> melees = new List<Character>();
    public Character[] MeleeCharacters => melees.ToArray();

    List<Character> supports = new List<Character>();
    public Character[] SupportCharacters => supports.ToArray();

    TeamAIHandler aiHandler;
    public TeamAIHandler AIHandler => aiHandler;

    //Timer aiDelayTimer;

    float aiActionsDelayTimer;
    float aiDelay = 1f;
    //bool delayingAI;
    UnityAction nextAIAction;

    bool isActive;
    public bool IsActive => isActive;
    
    void Awake()
    {
        characters = new Character[characterGameObjects.Length];

        InitializeCharacters();
        RepositionAnchors();

        aiHandler = new TeamAIHandler(this);
        aiHandler.AIFinishedEvent.AddListener(FinishTurn);

        //aiDelayTimer = Timer.GetInstance(aiDelay, DoNextAIAction);
    }

    public void StartTurn()
    {
        foreach (Character character in characters)
        {
            character.StartTurn();
        }
        aiHandler.StartAIActions();

        isActive = true;
        Changed();
    }

    void FinishTurn()
    {
        foreach (Character character in characters)
        {
            character.FinishTurn();
        }
        isActive = false;
        Changed();

        combatController.RotateTurns();
    }

    public void UpdateCharacterPositionListing(Character character)
    {
        if (character.PositionHandler.Position == TeamPosition.Melee)
        {
            supports.Remove(character);
            melees.Add(character);
        }
        else
        {
            melees.Remove(character);
            supports.Add(character);
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
            characters[i] = new Character(teamTemplate.CharacterSlots[i],
                this,
                characterGameObjects[i].GetComponent<CharacterActor>(),
                characterGameObjects[i].GetComponent<CharacterMover>());

            characters[i].PositionHandler.ListenToPosition(() =>
            {
                RepositionAnchors();
            });

            characters[i].PositionHandler.OnReposition.AddListener(UpdateCharacterPositionListing);

            List<Character> listToAddTo = characters[i].PositionHandler.Position == TeamPosition.Melee ? melees : supports;
            listToAddTo.Add(characters[i]);
        }
    }

    //void ControlAI()
    //{
    //    var aiPrioritized = characters.OrderByDescending(c => c.AIHandler.AITemplate.TurnPriority);

    //    var first = aiPrioritized.First();
    //    first.AIHandler.DoAIAction();
    //    for (int i = 1; i < aiPrioritized.Count(); i++)
    //    {
    //        var ai = aiPrioritized.ElementAt(i).AIHandler;
    //        var previous = aiPrioritized.ElementAt(i - 1).AIHandler;
    //        previous.WhenDoneActions(() => DoAIAction(ai.DoAIAction));

    //        if (i == aiPrioritized.Count() - 1)
    //        {
    //            ai.WhenDoneActions(() => DoAIAction(SignalFinished));
    //        }
    //    }
    //}

    //void DoNextAIAction()
    //{
    //    aiDelayTimer.Stop();
    //    nextAIAction.Invoke();
    //}

    //void DoAIAction()
    //{
    //    aiDelayTimer.Reset();
    //    aiDelayTimer.Start();
    //    nextAIAction = nextAction;
    //}
}

public enum TeamPosition
{
    Melee, Support
}
