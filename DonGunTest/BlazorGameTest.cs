using DonBlazor;
using System;
using NUnit.Framework;
using DonBlazor.Models;

namespace DonGunTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
           
           
        }

        [Test]
        public void Is_Game_Created_Correctly()
        {
            // Arrange
            Game game = new Game();
            
            // Act

            //Assert
            Assert.That(game != null, Is.True);
        }

        [Test]
        public void NextTurn_Adds_1_Turn()
        {
            // Arrange
            DonBlazor.Containers.ActiveGameContainer.CurrentTurn = 0;
            // Act
            DonBlazor.Containers.ActiveGameContainer.NextTurn();

            //Assert
            Assert.That(DonBlazor.Containers.ActiveGameContainer.CurrentTurn, Is.EqualTo(1));
        }


    }
}