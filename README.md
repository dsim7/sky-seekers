# SkySeekers

This is a prototype for an RPG style game with turn-based combat. The focus of this project was to architect a flexible, modular ability system. The modularity of the system is achieved using Scriptable Objects.

Look in Assets/_ScriptableObjects/Abilities to see how abilities are composed together using various Scriptable Object components configured in different ways. Components can be swapped out for one another to achieve various unique ability effects with minimal coding and recoding.

## Ability System

#### Ability
Most importantly, an Ability has a targeting type and has a list of Actions which are carried out sequentially.

#### Action
An Action has a list of Effects which all occur once the Action completes.

#### Effect
An Effect is some change to the character. For example, dealing damage, or applying a Status.

#### Status
A Status is a lingering effect on the character. They may cause some Effect on the target when they are applied, on each of their turns, and/or when the Status ends. A Status may also have a list of Modifiers which are applied when the status is applied and are removed when the status is removed.

#### Modifier
A Modifier will listen to certain events that a character triggers and modify those event and may even cause Effects to occur.
Examples:
-The "Expose Weakness" modifier listens to the target's events of receiving attacks and will modify those attacks to deal more damage.
-The "Poisoned Weapon" modifier listens to the target's events of attacking and modify those attacks to cause another separate damage Effect on the target. The attacks also have a chance to cause an Effect which applies the "Poisoned" Status on the target.

## Other Features

#### Action Point
Abilities cost Action Points to use. Some cost 1, others 2.

#### Positioning
There are two positions a character can be in during combat: Melee or Support. Depending on their position, they may use different Abilities and may be targeted by different Abilities. It costs 1 action points to reposition.

Importantly, on a team, there must be at least one character in Melee at all times. The last character in Melee on a team may not reposition.

#### Turn System
When you have decided your turn is complete, press the "Finish Turn" button and it will be the opponents turn.

#### AI
There is a basic AI in place for the opponent that will have them use Abilities with their characters in a reasonable and human-like fashion (as in not moving all their characters at once even though they can), before ending their turn and passing it back to you.

## To Do

#### Death and Combat End
At the moment, characters will not die when they reach 0 hp and combat will not end.

#### Displaying Statuses
There is currently no indication of what Statuses a character is under.

#### Sky Seekers
The original concept for this game was to have various characters, each with their own arsenal of abilities which complemented each other and facilitated teammate between characters. The arsenals of each character would be unlocked using various powerups acquired while playing. A narrative accompanying the game would be along the lines of high fantasy and dragons.
