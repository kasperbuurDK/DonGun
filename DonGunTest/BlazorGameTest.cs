using DonBlazor;
using System;
using NUnit.Framework;
using DonBlazor.Models;
using DonBlazor.Containers;
using SharedClassLibrary;
using System.Numerics;
using SharedClassLibrary.Exceptions;

namespace DonGunTest
{
    public class BlazorGameTest
    {
        private ActiveGameContainer activeGame;

        [SetUp]
        public void SetUp()
        {
            activeGame = ActiveGameContainer.GetGameInstance;
            activeGame.Name = "Active test";
            activeGame.HumanPlayers = new List<Player> { };
            activeGame.NonHumanPlayers = new List<Npc> { };
            activeGame.Created = DateTime.Now; 
            activeGame.LastSaved = DateTime.Now;
            activeGame.CurrentTurn = 999;
        }


        [Test]
        public void Is_Game_Created_Correctly()
        {
            //Assert
            Assert.That(activeGame, Is.Not.EqualTo(null));
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

        [TestCase(1)]
        [TestCase(5)]
        public void Adding_New_Players_Result_In_Correct_Number (int numberOfPlayersToAdd) 
        {
            for (int i = 0; i < numberOfPlayersToAdd; i++) 
            {
                Player player = new Player($"player{i}");
                activeGame.AddPlayerToGame(player);
            }

            Assert.That(activeGame.HumanPlayers.Count, Is.EqualTo(numberOfPlayersToAdd));
        }

        [Test]
        public void Destroy_Activegame_Results_in_an_empty_game()
        {
            // Act
            activeGame.DestroyGameInstance();
            activeGame = ActiveGameContainer.GetGameInstance;

            Assert.That(activeGame.Name, Is.EqualTo("Empty Game"));
        }

        [Test]
        public void Determine_Correct_Character()
        {
            int numberOfPlayers = 2;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Player player = new Player($"player{i}");
                activeGame.AddPlayerToGame(player);
            }
            activeGame.CurrentTurn = numberOfPlayers;

            Assert.That(activeGame.CharacterToAct, Is.EqualTo(0));
        }

        [Test]
        public void Determine_Character_Without_Players_Throws_Exception() 
        {
            activeGame.HumanPlayers = new List<Player>() { };
            activeGame.NonHumanPlayers = new List<Npc>() { };

            Assert.Throws<NoPLayersInGameException>(() => { int testValue = activeGame.CurrentCharacter; });
        }

        [Test]
        public void AllCharacters_Are_The_Sum_Of_Human_And_NPCs()
        {
            int noOfHumans = 2;
            int noOfNPCs = 6;

            activeGame.HumanPlayers = new List<Player>() { };
            activeGame.NonHumanPlayers = new List<Npc>() { };

            for (int i = 0; i < noOfHumans; i++)
            {
                Player player = new($"player{i}");
                activeGame.HumanPlayers.Add(player);
            }

            for (int i = 0; i < noOfNPCs; i++)
            {
                Npc npc = new();
                activeGame.NonHumanPlayers.Add(npc);
            }

            Assert.That(activeGame.AllCharacters, Has.Count.EqualTo(noOfHumans + noOfNPCs));
        }
    }
}