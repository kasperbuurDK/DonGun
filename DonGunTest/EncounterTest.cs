using SharedClassLibrary;
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
        }


        [Test]
        public void Character_moves_correct_to_East()
        {
            _mainCharacter.Move('E', 1);
            Assert.That(_mainCharacter.Position, Is.EqualTo(new Position(1, 0)));
        }

        [Test]
        public void Character_moves_correct_to_West()
        {
            _mainCharacter.Move('W', 1);
            Assert.That(_mainCharacter.Position, Is.EqualTo(new Position(-1, 0)));
        }



    }
}

