
using SharedClassLibrary.AuxUtils;
using SharedClassLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClassLibrary;

namespace SharedClassLibrary
{

    public class GameMaster
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
            List<Character> othersInSight = new List<Character>();

            foreach (Character otherCharacter in _game.AllCharacters)
            {
                if (otherCharacter.Equals(character)) continue;

                float distanceToOther = DetermineDistanceBetweenCharacters(character, otherCharacter);
                if (character.SightRange >= distanceToOther )
                {
                    othersInSight.Add(otherCharacter);
                }
            }

            character.OthersInSight = othersInSight;
            character.UpdatePossibleActions();
        }

        private float DetermineDistanceBetweenCharacters(Character character, Character otherCharacter)
        {
            float deltaX = 
                Math.Max(character.Position.X, otherCharacter.Position.X) - Math.Min(character.Position.X, otherCharacter.Position.X);
            float deltaY = 
                Math.Max(character.Position.Y, otherCharacter.Position.Y) - Math.Min(character.Position.Y, otherCharacter.Position.Y);

            if (deltaX == 0) return deltaY;
            if (deltaY == 0) return deltaX;

            float distanceBetween = (float) Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            return distanceBetween;
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
            if (characterToAdd is Player)  
            {
                _game.HumanPlayers.Add((Player)characterToAdd);
            }
            else if (characterToAdd is Npc) 
            {
                _game.NonHumanPlayers.Add((Npc)characterToAdd);
            }
        }
    }
}
