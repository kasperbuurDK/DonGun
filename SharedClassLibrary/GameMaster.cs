
using SharedClassLibrary.Actions;
using SharedClassLibrary.AuxUtils;
using SharedClassLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary
{

    public class GameMaster : GameMasterHelpers
    {
        private Game _game;

        public GameMaster(Game game) { _game = game; }

        public string Move(Character character, MoveDirections direction, int distance)
        {
            if (character.Facing != direction)
            {
                int costOfTurn = CostOfTurningCharacter(character, direction);
                if (costOfTurn > character.MpCur)
                {
                    return MessageStrings.StandardMessages.NotEnough("MP", "turn" );
                }
                else
                {
                    character.MpCur -= costOfTurn;
                    character.Facing = direction;
                }
            }

            if (character.MpCur < distance * MoveHelper.COSTOFSSINGLEMOVE)
            {
                return MessageStrings.StandardMessages.NotEnough("MP", "move"); ;
            }
            else
            {
                character.MpCur -= distance * MoveHelper.COSTOFSSINGLEMOVE;
            }

            Position newPos = direction switch
            {
                MoveDirections.North => new Position(character.Position.X, character.Position.Y + distance),
                MoveDirections.East => new Position(character.Position.X + distance, character.Position.Y),
                MoveDirections.South => new Position(character.Position.X, character.Position.Y - distance),
                MoveDirections.West => new Position(character.Position.X - distance, character.Position.Y),
                _ => throw new WrongInputToFunction(),
            };

            character.Position = newPos;
            DetermineOthersInSight(character);

            return MessageStrings.StandardMessages.AllOK;
        }

        private void DetermineOthersInSight(Character character)
        {
            List<string> othersInSight = new List<string>();

            foreach (Character otherCharacter in _game.AllCharacters)
            {
                if (otherCharacter.Equals(character)) continue;

                float distanceToOther = DetermineDistanceBetweenCharacters(character, otherCharacter);
                if (character.SightRange >= distanceToOther )
                {
                    othersInSight.Add(otherCharacter.Signature);
                }
            }

            character.OthersInSight = othersInSight;
            UpdatePossibleActions(character);
        }

        public void UpdatePossibleActions(Character character)
        {
            if (character.OthersInSight != null)
            {
                character.PossibleHelperActions = new List<HelperAction>();
                character.PossibleOffensiveActions = new List<OffensiveAction>();
                foreach (string otherSignature in character.OthersInSight)
                {
                    var otherCharacter = _game.AllCharacters.Find(chara => chara.Signature == otherSignature);
                    if (otherCharacter == null) continue;

                    if (otherCharacter.Team == character.Team)
                    {
                        HealAlly healAction = new();
                        healAction.Sender = character;
                        healAction.Reciever = otherCharacter;
                        character.PossibleHelperActions.Add(healAction);

                        InspireAlly inspireAction = new();
                        healAction.Sender = character;
                        healAction.Reciever = otherCharacter;
                        character.PossibleHelperActions.Add(inspireAction);

                    }
                    else if (otherCharacter.Team != character.Team)
                    {
                        OffensiveAction anOffensiveAction = new();
                        int distToOther = (int)Math.Floor(GameMasterHelpers.DetermineDistanceBetweenCharacters(character, otherCharacter));
                        int baseChance = distToOther >  character.HitModifierProfile.Length ? -100000 : character.HitModifierProfile[distToOther];
                        int dexModifier = GameMasterHelpers.RandomRange(0, character.Dexterity) * 2;
                        anOffensiveAction.Sender = character;
                        anOffensiveAction.Reciever = otherCharacter;
                        anOffensiveAction.ChanceToSucced = baseChance + dexModifier;
                        character.PossibleOffensiveActions.Add(anOffensiveAction);
                    }

                }
                character.PossibleActions.Clear();
                character.PossibleActions.AddRange(character.PossibleHelperActions);
                character.PossibleActions.AddRange(character.PossibleOffensiveActions);
            }
        }

        private int CostOfTurningCharacter(Character character, MoveDirections turnTowards)
        {
            int costOfturning = 0;

            switch (turnTowards)
            {
                case MoveDirections.North:
                    costOfturning = character.Facing == MoveDirections.South ? 2 : 1;
                    break;
                case MoveDirections.East:
                    costOfturning = character.Facing == MoveDirections.West ? 2 : 1;
                    break;
                case MoveDirections.South:
                    costOfturning = character.Facing == MoveDirections.North ? 2 : 1;
                    break;
                case MoveDirections.West:
                    costOfturning = character.Facing == MoveDirections.East ? 2 : 1;
                    break;
                default:
                    break;
            }
            return costOfturning;
        }

        public void AddCharacterToGame(Character characterToAdd) 
        {
            SetMaxValuesBasedOnMainStats(characterToAdd);

            if (characterToAdd is Player)  
            {
                _game.HumanPlayers.Add((Player)characterToAdd);
            }
            else if (characterToAdd is Npc) 
            {
                _game.NonHumanPlayers.Add((Npc)characterToAdd);
            }
        }

        public void SetMaxValuesBasedOnMainStats(Character character)
        {
            character.HealthMax = 50 + character.Constitution * 2;
            character.SightRange = 5 + character.Intelligence / 3;
            character.ResourceMax = character.Wisdome * 2;
            
        }
    }
}
