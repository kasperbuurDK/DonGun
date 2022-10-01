using DonBlazor;
using System;
using NUnit.Framework;
using DonBlazor.Models;
using DonBlazor.Containers;

namespace DonGunTest
{
    public class BlazorGameTest
    {
        private ActiveGameContainer activeGame;

        [SetUp]
        public void SetUp()
        {
            activeGame = ActiveGameContainer.GetGameInstance; 
        }


        [Test]
        public void Is_Game_Created_Correctly()
        {
          
            //Assert
            Assert.That(activeGame != null, Is.True);
        }

        [Test]
        public void NextTurn_Adds_1_Turn()
        {
            // Arrange
            activeGame.CurrentTurn = 0;
            // Act
            activeGame.NextTurn();

            //Assert
            Assert.That(activeGame.CurrentTurn, Is.EqualTo(1));
        }

        [Test]
        public void Destroy_Activegame_Results_in_an_empty_game()
        {
            // Arrange
            ActiveGameContainer.DestroyGameInstance();
            

            Assert.That(activeGame.Name, Is.EqualTo("Empty Game"));
        }


    }
}