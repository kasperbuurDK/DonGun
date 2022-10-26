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

        [SetUp]
        public void Setup()
        {
            _mainCharacter = new Player("Main Player");
            _mainCharacter.Position = new Position(0, 0);
            _mainCharacter.Mp = 10;
            _mainCharacter.MpCur = _mainCharacter.Mp;
        }

        [TestCase(MoveDirections.North, 1, 0, 1)]
        [TestCase(MoveDirections.East, 1, 1, 0)]
        [TestCase(MoveDirections.South, 1, 0, -1)]
        [TestCase(MoveDirections.West, 1, -1, 0)]

        public void Character_moves_correct(SharedClassLibrary.AuxUtils.MoveDirections direction, int distance, int endX, int endY)
        {
            _mainCharacter.Move(direction, distance);
            Assert.That(_mainCharacter.Position, Is.EqualTo(new Position(endX, endY)));
        }

        [Test]
        public void Moving_consumes_Correct_MP()
        {
            _mainCharacter.Move(MoveDirections.North, 1);
            Assert.That(_mainCharacter.MpCur, Is.EqualTo(8));
        }

        [Test]
        public void Turning_consumes_correct_MP()
        {
            // Arrange
            _mainCharacter.Facing = MoveDirections.East;

            // Act
            _mainCharacter.Move(MoveDirections.South, 0);
            
            // Assert
            Assert.That(_mainCharacter.MpCur, Is.EqualTo(9));
        }

       

    }
}

