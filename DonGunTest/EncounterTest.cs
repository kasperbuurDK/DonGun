using SharedClassLibrary;
using SharedClassLibrary.Exceptions;
using SharedClassLibrary.AuxUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Position = new Position(0, 0),
                Mp = 10,
            };
            _mainCharacter.MpCur = _mainCharacter.Mp;

            _game = new Game();
            _game.HumanPlayers.Add(_mainCharacter);

            _gameMaster = new GameMaster(_game);

        }

        [TestCase(MoveDirections.North, 1, 0, 1)]
        [TestCase(MoveDirections.East, 1, 1, 0)]
        [TestCase(MoveDirections.South, 1, 0, -1)]
        [TestCase(MoveDirections.West, 1, -1, 0)]

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
        
        [Test]
        public void Current_character_can_see_another_character() 
        {
            _gameMaster.Move(_mainCharacter, MoveDirections.North, 0);



            Assert.That(_mainCharacter.OthersInSight.Count, Is.EqualTo(1));
        }



      
    }
}

