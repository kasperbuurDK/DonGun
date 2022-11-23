using SharedClassLibrary;
using SharedClassLibrary.Exceptions;
using SharedClassLibrary.AuxUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedClassLibrary.Actions;
using DonBlazor.Client;

namespace DonGunTest
{
    /// <summary>
    /// Test class to test what a character can do on a turn in an encounter
    /// </summary>
    internal class EncounterTest
    {
        private Player _mainCharacter;
        private GameMaster _gameMaster;
        private Game _game;

        [SetUp]
        public void Setup()
        {
            _mainCharacter = new Player("Main Player")
            {
                Position = new Position(10, 10),
                MpMax = 10,
                Team = 0,
                SightRange = 2,
            };
            _mainCharacter.MpCur = _mainCharacter.MpMax;

            _game = new Game();
            _gameMaster = new GameMaster(_game);
            _gameMaster.AddCharacterToGame(_mainCharacter);


        }

        private void CreateNewEnemyNearby()
        {
            _gameMaster.AddCharacterToGame(new Npc()
            {
                Position = new Position(_mainCharacter.Position.X - 1, _mainCharacter.Position.Y - 1),
                Team = _mainCharacter.Team +1,
            });

        }

        private void CreateNewFriendNearby()
        {
            _gameMaster.AddCharacterToGame(new Player("Friend")
            {
                Position = new Position(_mainCharacter.Position.X + 1, _mainCharacter.Position.Y + 1),
                Team = _mainCharacter.Team,
            });

        }

        [TestCase(MoveDirections.North, 1, 10, 11)]
        [TestCase(MoveDirections.East, 1, 11, 10)]
        [TestCase(MoveDirections.South, 1, 10, 9)]
        [TestCase(MoveDirections.West, 1, 9, 10)]

        public void Character_moves_correct(MoveDirections direction, int distance, int endX, int endY)
        {
            _gameMaster.Move(_mainCharacter, direction, distance);
            
            Assert.That(_mainCharacter.Position, Is.EqualTo(new Position(endX, endY)));
        }

        [Test]
        public void Moving_straight_consumes_Correct_MP()
        {

            // Arrange
            _mainCharacter.Facing = MoveDirections.North;

            // Act
            _gameMaster.Move(_mainCharacter, MoveDirections.North, 1);
            
            // Assert
            Assert.That(_mainCharacter.MpCur, Is.EqualTo(8));
        }

        [TestCase(MoveDirections.North, MoveDirections.South, 2)]
        [TestCase(MoveDirections.North, MoveDirections.East, 1)]
        [TestCase(MoveDirections.North, MoveDirections.East, 1)]
        [TestCase(MoveDirections.North, MoveDirections.North, 0)]
        
        [TestCase(MoveDirections.South, MoveDirections.North, 2)]
        [TestCase(MoveDirections.South, MoveDirections.West, 1)]
        [TestCase(MoveDirections.South, MoveDirections.East, 1)]
        [TestCase(MoveDirections.South, MoveDirections.South, 0)]
        
        [TestCase(MoveDirections.East, MoveDirections.West, 2)]
        [TestCase(MoveDirections.East, MoveDirections.North, 1)]
        [TestCase(MoveDirections.East, MoveDirections.South, 1)]
        [TestCase(MoveDirections.East, MoveDirections.East, 0)]
        
        [TestCase(MoveDirections.West, MoveDirections.East, 2)]
        [TestCase(MoveDirections.West, MoveDirections.North, 1)]
        [TestCase(MoveDirections.West, MoveDirections.South, 1)]
        [TestCase(MoveDirections.West, MoveDirections.West, 0)]
         
        public void Turning_consumes_correct_MP(MoveDirections startFacing, MoveDirections turnTo, int expectedCost)
        {
            // Arrange
            _mainCharacter.Facing = startFacing;

            // Act
            _gameMaster.Move(_mainCharacter, turnTo, 0);
            
            // Assert
            Assert.That(_mainCharacter.MpCur, Is.EqualTo(10 - expectedCost));
        }

        [TestCase(4, 10)]
        public void Turning_AND_moving_consumes_correct_MP(int distance, int expectedCost)
        {
            // Arrange
            _mainCharacter.Facing = MoveDirections.North;

            // Act
            string moveStatus = _gameMaster.Move(_mainCharacter, MoveDirections.South, 4);
            
            // Assert
            Assert.That(_mainCharacter.MpCur, Is.EqualTo(10 - expectedCost));
        }

        [Test]
        public void Using_to_many_MP_is_not_OK() 
        {
            _mainCharacter.Facing = MoveDirections.North;
              
            Assert.That(_gameMaster.Move(_mainCharacter, MoveDirections.North, 20), Is.Not.EqualTo("OK"));
        }
        
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        public void Current_character_see_all_in_range(int noOfOtherCharacteres) 
        {
            _mainCharacter.Position = new Position(10, 10);
            _mainCharacter.SightRange = 100;

            for (int i = 0; i < noOfOtherCharacteres; i++)
            {
                Character otherCharacter = new Npc()
                {
                    Position = new Position(9, 9),
                };

                _gameMaster.AddCharacterToGame(otherCharacter);
            }
            
            _gameMaster.Move(_mainCharacter, MoveDirections.North, 0);
            Assert.That(_mainCharacter.OthersInSight.Count, Is.EqualTo(noOfOtherCharacteres));
        }
        
        [Test]
        public void Current_character_dont_see_any_when_there_are_none_in_range() 
        {

            _mainCharacter.SightRange = 2;
            _mainCharacter.Position = new Position(10, 10);

            _gameMaster.AddCharacterToGame(new Npc() { Position = new Position(10, 7) });

            _gameMaster.Move(_mainCharacter, MoveDirections.North, 0);
            Assert.That(_mainCharacter.OthersInSight.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Current_character_only_see_others_in_range()
        {
            CreateTwoNPSInSightAndOneOutOfSight();

            _gameMaster.Move(_mainCharacter, MoveDirections.North, 0);
            Assert.That(_mainCharacter.OthersInSight.Count, Is.EqualTo(2));
        }

        private void CreateTwoNPSInSightAndOneOutOfSight()
        {
            _mainCharacter.SightRange = 2;
            _mainCharacter.Position = new Position(10, 10);

            _gameMaster.AddCharacterToGame(new Npc() { Position = new Position(10, 7) }); // Not in range
            _gameMaster.AddCharacterToGame(new Npc() { Position = new Position(10, 9) }); // In range
            _gameMaster.AddCharacterToGame(new Npc() { Position = new Position(9, 9) }); // In range
        }

        [Test]
        public void If_teammember_in_sight_character_can_make_a_HelpAction() 
        {
            CreateNewFriendNearby();
            _gameMaster.Move(_mainCharacter, MoveDirections.North, 0);
     
            Assert.That(_gameMaster.PossibleHelperActions?.Count, Is.GreaterThan(0));
        }
        
        [Test]
        public void If_non_team_member_in_sight_character_can_make_an_OffensiveAction() 
        {
            CreateNewEnemyNearby();
            _gameMaster.Move(_mainCharacter, MoveDirections.North, 0);
        
            Assert.That(_gameMaster.PossibleOffensiveActions?.Count, Is.GreaterThan(0));
        }

        

        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        public void Taking_offensive_action_hits_approx_as_much_as_ChanceToHit(int distanceToMainChar) 
        {
            int timesTakingAction = 1000000;

            Npc npc = new Npc() 
            { 
                Position = new Position(_mainCharacter.Position.X - distanceToMainChar, _mainCharacter.Position.Y),
                Team = 1,
            };
            _gameMaster.AddCharacterToGame(npc);
            _mainCharacter.SightRange = 200;
            _gameMaster.Move(_mainCharacter, MoveDirections.North, 0);

            int timesHit = 0;
            int diceValue = 10;
            
            Parallel.For(0, timesTakingAction,
                           index => {
                               if (_gameMaster.PossibleOffensiveActions[0].MakeBasicAction(diceValue, _mainCharacter, npc)) {
                                     Interlocked.Add(ref timesHit, 1); }
                           }
                           );
            
            
            int minBoundary = (int)(timesTakingAction * _gameMaster.PossibleOffensiveActions[0].ChanceToSucced/100 * 0.98f);
            int maxBoundary = (int)(timesTakingAction * _gameMaster.PossibleOffensiveActions[0].ChanceToSucced/100 * 1.02f);

            if (maxBoundary > timesTakingAction) { maxBoundary = timesTakingAction; }
            if (minBoundary > timesTakingAction) { minBoundary = timesTakingAction; }


            Assert.That(timesHit, Is.AtLeast(minBoundary));
            Assert.That(timesHit, Is.AtMost(maxBoundary));
        }

        [Test]
        public void Hitting_character_applies_damage()
        {

            int startHealth = 100;
            int diceValue = 10;
         
            CreateNewEnemyNearby();
            Npc enemyCharacter = _game.NonHumanPlayers[0];
            enemyCharacter.HealthMax = startHealth;

            _gameMaster.Move(_mainCharacter, MoveDirections.North, 0);
            while (!_gameMaster.PossibleOffensiveActions[0].MakeBasicAction(diceValue, _mainCharacter, enemyCharacter))
            
            Assert.That(enemyCharacter.HealthCurrent, Is.LessThan(startHealth));
        }


        [Test]
        public void Player_can_help_Ally()
        {
            CreateNewFriendNearby();
            _gameMaster.Move(_mainCharacter, MoveDirections.North, 0);

            Assert.That(_gameMaster.PossibleHelperActions.Count, Is.GreaterThan(0));
        }

        
        [Test]
        public void Player_can_heal_Ally()
        {
            int startHealth = 1;
            int diceValue = 10;
            CreateNewFriendNearby();
            Player friend = _game.HumanPlayers.Find(player => player.Name == "Friend");
            friend.HealthCurrent = startHealth;
            _gameMaster.Move(_mainCharacter, MoveDirections.North, 0);

            var actions = _gameMaster.PossibleHelperActions;
            var theSig = _mainCharacter.PossibleHelperActionsSignatures[0];

            var theAction = actions.Find(act => act.Signature == theSig); 
               
            theAction.MakeBasicAction(diceValue, _mainCharacter, friend);

            Assert.That(friend.HealthCurrent, Is.GreaterThan(startHealth));
        }
        



    }
}