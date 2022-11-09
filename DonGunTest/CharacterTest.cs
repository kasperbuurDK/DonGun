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

        Player player;

        [SetUp]
        public void SetUp() 
        {
            player = new Player("Test Plsyer");
        }



        [TestCase(0, 50)]
        [TestCase(5, 60)]
        [TestCase(10, 70)]
        [TestCase(15, 80)]
        [TestCase(20, 90)]

        public void Player_has_correct_health(int consScore, int expectedResult)
        {
            
            player.Constitution = consScore;

            player.SetMaxValuesBasedOnMainStats();
          
            Assert.That(player.HealthMax, Is.EqualTo(expectedResult));
        }
        
        [TestCase(0,5)]
        [TestCase(5,6)]
        [TestCase(10,8)]
        [TestCase(15,10)]
        [TestCase(20,11)]
       
        public void Player_has_correct_Sightrange(int intScore, int expectedResult)
        {
            player.Intelligence = intScore;
            player.SetMaxValuesBasedOnMainStats();

            Assert.That(player.SightRange, Is.EqualTo(expectedResult));
        }
        
        [TestCase(0,0)]
        [TestCase(5,10)]
        [TestCase(10,20)]
        [TestCase(15,30)]
        [TestCase(20,40)]
       
        public void Player_has_correct_RessourceMax(int wisScore, int expectedResult)
        {
            player.Wisdome = wisScore;
            player.SetMaxValuesBasedOnMainStats();

            Assert.That(player.ResourceMax, Is.EqualTo(expectedResult));
        }

    }
}
