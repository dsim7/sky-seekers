using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterTeamHandler : ObservedObject
{
    Character owner;
    
    Team team;
    public Team Team => team;

    int costToReposition = 1;
    public int CostToReposition => costToReposition;

    TeamPosition position;
    public TeamPosition Position => position;

    bool _isLoneMelee;
    bool isLoneMelee { get { return _isLoneMelee; } set { _isLoneMelee = value; Changed(); } }
    int _preventingReposition;
    int preventingReposition { get { return _preventingReposition; } set { _preventingReposition = value; Changed(); } }
    public bool CanReposition => !isLoneMelee && preventingReposition == 0;

    public CharacterTeamHandler(Character character, Team team, TeamPosition position)
    {
        owner = character;
        this.position = position;
        this.team = team;
    }

    public void Reposition()
    {
        if (CanReposition)
        {
            position = position == TeamPosition.Melee ? TeamPosition.Support : TeamPosition.Melee;
            team.RepositionAnchors();
            
            EnsureLastMeleeCantReposition();

            Changed();
        }
    }

    public void PreventReposition()
    {
        preventingReposition += 1;
    }

    public void AllowReposition()
    {
        preventingReposition -= 1;
    }

    void EnsureLastMeleeCantReposition()
    {
        var meleeChars = team.Characters.Where(c => c.TeamHandler.Position == TeamPosition.Melee);

        if (meleeChars.Count() == 1)
        {
            meleeChars.First().TeamHandler.isLoneMelee = true;
        }
        else
        {
            foreach (var c in meleeChars)
            {
                c.TeamHandler.isLoneMelee = false;
            }
        }
    }
}

public class TeamPositionVariable : ObservedVariable<TeamPosition> { }