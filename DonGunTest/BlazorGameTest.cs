using DonBlazor.Client;
using DonBlazor.Containers;
using NUnit.Framework;
using SharedClassLibrary;


namespace DonGunTest
{
    public class BlazorGameTest
    {
      //  private ActiveGameContainer _activeGame;
        private ActiveGameMasterContainer _gameMaster;


        [SetUp]
        public void SetUp()
        {

         //   _activeGame = ActiveGameContainer.GetGameInstance;
            _gameMaster = ActiveGameMasterContainer.GetGameMasterInstance;
            //_gameMaster.AddActiveGame();

            _gameMaster.Game.Name = "Active test";
            _gameMaster.Game.HumanPlayers = new List<Player> { };
            _gameMaster.Game.NonHumanPlayers = new List<Npc> { };
            _gameMaster.Game.Created = DateTime.Now;
            _gameMaster.Game.LastSaved = DateTime.Now;
            _gameMaster.Game.CurrentTurn = 999;
        }


        [Test]
        public void Gamemaster_has_the_right_gameinstance()
        {   
             Assert.That(_gameMaster.Game, Is.EqualTo(ActiveGameContainer.GetGameInstance));
        }

     
        [TestCase(1)]
        [TestCase(5)]
        public void Adding_New_Players_Result_In_Correct_Number(int numberOfPlayersToAdd)
        {
            for (int i = 0; i < numberOfPlayersToAdd; i++)
            {
                Player player = new Player($"player{i}");
                _gameMaster.AddCharacterToGame(player);
            }

            Assert.That(_gameMaster.Game.HumanPlayers.Count, Is.EqualTo(numberOfPlayersToAdd));
        }

        [Test]
        public void AllCharacters_Are_The_Sum_Of_Human_And_NPCs()
        {
            int noOfHumans = 2;
            int noOfNPCs = 6;

            _gameMaster.Game.HumanPlayers = new List<Player>() { };
            _gameMaster.Game.NonHumanPlayers = new List<Npc>() { };

            for (int i = 0; i < noOfHumans; i++)
            {
                Player player = new($"player{i}");
                _gameMaster.Game.HumanPlayers.Add(player);
            }

            for (int i = 0; i < noOfNPCs; i++)
            {
                Npc npc = new();
                _gameMaster.Game.NonHumanPlayers.Add(npc);
            }

            Assert.That(_gameMaster.Game.AllCharacters, Has.Count.EqualTo(noOfHumans + noOfNPCs));
        }
    }
}