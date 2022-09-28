using DonBlazor;
using System;
using NUnit.Framework;
using DonBlazor.Models;
using DonBlazor.Containers;

namespace DonGunTest
{
    public class BlazorGameTest
    {
        /*
        [SetUp]
        public void Setup()
        {

        }
        */

        [Test]
        public void Is_Game_Created_Correctly()
        {
            // Arrange
            ActiveGameContainer game = ActiveGameContainer.GetGameInstance;


            // Act

            //Assert
            Assert.That(game != null, Is.True);
        }

        [Test]
        public void NextTurn_Adds_1_Turn()
        {
            // Arrange
            ActiveGameContainer game = ActiveGameContainer.GetGameInstance;
            game.CurrentTurn = 0;
            // Act
            game.NextTurn();

            //Assert
            Assert.That(game.CurrentTurn, Is.EqualTo(1));
        }


    }
}