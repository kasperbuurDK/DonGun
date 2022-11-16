
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

        public GameMaster()
        {

        }
        public GameMaster(Game game) { _game = game; }

        private List<IAnAction>? _possibleActions = new() { };
        private List<HelperAction>? _possibleHelperActions = new() { };
        private List<OffensiveAction>? _possibleOffensiveActions = new() { };

        public List<IAnAction>? PossibleActions { get => _possibleActions; set => _possibleActions = value; }
        public List<HelperAction>? PossibleHelperActions { get => _possibleHelperActions; set => _possibleHelperActions = value; }
        public List<OffensiveAction>? PossibleOffensiveActions { get => _possibleOffensiveActions; set => _possibleOffensiveActions = value; }
        public Queue<Character> Queue { get; set; }

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
                _possibleHelperActions = new List<HelperAction>();
                _possibleOffensiveActions = new List<OffensiveAction>();
                _possibleActions = new List<IAnAction>();

                character.PossibleHelperActionsSignatures = new List<string>();
                character.PossibleOffensiveActionsSignatures = new List<string>();
                foreach (string otherSignature in character.OthersInSight)
                {
                    var otherCharacter = _game.AllCharacters.Find(chara => chara.Signature == otherSignature);
                    if (otherCharacter == null) continue;

                    if (otherCharacter.Team == character.Team)
                    {
                        HealAlly healAction = new HealAlly(character.Signature, otherSignature);
                        healAction.SenderSignature = character.Signature;
                        healAction.RecieverSignature = otherSignature;
                        _possibleHelperActions.Add(healAction);
                        character.PossibleHelperActionsSignatures.Add(healAction.Signature);

                        InspireAlly inspireAction = new InspireAlly(character.Signature, otherSignature);
                        inspireAction.SenderSignature = character.Signature;
                        inspireAction.RecieverSignature = otherSignature;
                        _possibleHelperActions.Add(inspireAction);
                        character.PossibleHelperActionsSignatures.Add(inspireAction.Signature);

                    }
                    else if (otherCharacter.Team != character.Team)
                    {
                        OffensiveAction anOffensiveAction = new OffensiveAction(character.Signature, otherSignature);
                        int distToOther = (int)Math.Floor(GameMasterHelpers.DetermineDistanceBetweenCharacters(character, otherCharacter));
                        int baseChance = distToOther >  character.HitModifierProfile.Length ? -100000 : character.HitModifierProfile[distToOther];
                        int dexModifier = GameMasterHelpers.RandomRange(0, character.Dexterity) * 2;
                        

                        anOffensiveAction.SenderSignature = character.Signature;
                        anOffensiveAction.RecieverSignature = otherCharacter.Signature;
                        anOffensiveAction.ChanceToSucced = baseChance + dexModifier;
                        character.PossibleOffensiveActionsSignatures.Add(anOffensiveAction.Signature);
                        _possibleOffensiveActions.Add(anOffensiveAction);
                    }

                }
                _possibleActions.Clear();
                _possibleActions.AddRange(_possibleHelperActions);
                _possibleActions.AddRange(_possibleOffensiveActions);
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

        public void StartEncounter()
        {
            Queue = new Queue<Character>(_game.AllCharacters);
            _game.CharacterToAct = Queue.Dequeue();
            Queue.Enqueue(_game.CharacterToAct);

        }

        public void EndTurn()
        {
            _game.CharacterToAct = Queue.Dequeue();
            Queue.Enqueue(_game.CharacterToAct);


        }
    }
}
