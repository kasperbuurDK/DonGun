using SharedClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonGunTest
{
    internal class CharacterTest
    {

        Player _player;
        GameMaster _gameMaster;
        Game _game;

        [SetUp]
        public void SetUp() 
        {
            _player = new Player("Test Plsyer");
            _game = new Game();
            _gameMaster = new GameMaster(_game);
            _gameMaster.AddCharacterToGame(_player);
        }



        [TestCase(0, 50)]
        [TestCase(5, 60)]
        [TestCase(10, 70)]
        [TestCase(15, 80)]
        [TestCase(20, 90)]

        public void Player_has_correct_health(int consScore, int expectedResult)
        {
            _player.Constitution = consScore;
            _gameMaster.SetMaxValuesBasedOnMainStats(_player);
            Assert.That(_player.HealthMax, Is.EqualTo(expectedResult));
        }
        
        [TestCase(0,5)]
        [TestCase(5,6)]
        [TestCase(10,8)]
        [TestCase(15,10)]
        [TestCase(20,11)]
       
        public void Player_has_correct_Sightrange(int intScore, int expectedResult)
        {
            _player.Intelligence = intScore;
            _gameMaster.SetMaxValuesBasedOnMainStats(_player);

            Assert.That(_player.SightRange, Is.EqualTo(expectedResult));
        }
        
        [TestCase(0,0)]
        [TestCase(5,10)]
        [TestCase(10,20)]
        [TestCase(15,30)]
        [TestCase(20,40)]
       
        public void Player_has_correct_RessourceMax(int wisScore, int expectedResult)
        {
            _player.Wisdome = wisScore;
            _gameMaster.SetMaxValuesBasedOnMainStats(_player);

            Assert.That(_player.ResourceMax, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Player_has_a_signature()
        {
            Assert.That(!String.IsNullOrEmpty(_player.Signature));
        }

    }
}
