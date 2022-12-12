using DonBlazor.Client;
using DonBlazor.Containers;
using Microsoft.AspNetCore.Components;
using SharedClassLibrary;



namespace DonGunTest
{
    public class ContainerTest
    {
        
        ActiveContainer container;


        [SetUp]
        public void SetUp()
        {
            container = new ActiveContainer();
        }

        [Test]
        public void Container_has_a_gameMaster()
        {
            Assert.That(container.GameMaster, Is.Not.Null);
        }

        [Test]
        public void Containers_GM_has_a_game()
        {
            Assert.That(container.GameMaster.Game, Is.Not.Null);
        }

        [Test]
        public void GameMasters_game_is_the_same_as_containers_game() 
        {
        
            Assert.That(container.GameMaster.Game, Is.EqualTo(container.Game));
        }

        [Test]
        public void GameMaster_is_always_the_same()
        {
            var first = container.GameMaster;
            var second = container.GameMaster;

            Assert.IsTrue(first.Equals(second));
        }

        
        [TestCase(1)]
        [TestCase(5)]
        public void Adding_New_Players_Result_In_Correct_Number(int numberOfPlayersToAdd)
        {
            for (int i = 0; i < numberOfPlayersToAdd; i++)
            {
                Player player = new Player($"player{i}");
                container.GameMaster.AddCharacterToGame(player);
            }

            Assert.That(container.GameMaster.Game.HumanPlayers.Count, Is.EqualTo(numberOfPlayersToAdd));
        }

        [Test]
        public void AllCharacters_Are_The_Sum_Of_Human_And_NPCs()
        {
            int noOfHumans = 2;
            int noOfNPCs = 6;

            container.GameMaster.Game.HumanPlayers = new List<Player>() { };
            container.GameMaster.Game.NonHumanPlayers = new List<Npc>() { };

            for (int i = 0; i < noOfHumans; i++)
            {
                Player player = new($"player{i}");
                container.GameMaster.Game.HumanPlayers.Add(player);
            }

            for (int i = 0; i < noOfNPCs; i++)
            {
                Npc npc = new();
                container.GameMaster.Game.NonHumanPlayers.Add(npc);
            }

            Assert.That(container.GameMaster.Game.AllCharacters, Has.Count.EqualTo(noOfHumans + noOfNPCs));
        }



    }
}
