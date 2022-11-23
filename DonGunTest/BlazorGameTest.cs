using DonBlazor.Client;
using DonBlazor.Containers;
using SharedClassLibrary;

namespace DonGunTest
{
    public class BlazorGameTest
    {
        private ActiveGameContainer _activeGame;
        private GameMaster _gameMaster;


        [SetUp]
        public void SetUp()
        {
            _gameMaster = new GameMaster();

            _activeGame = ActiveGameContainer.GetGameInstance;
            _activeGame.Name = "Active test";
            _activeGame.HumanPlayers = new List<Player> { };
            _activeGame.NonHumanPlayers = new List<Npc> { };
            _activeGame.Created = DateTime.Now;
            _activeGame.LastSaved = DateTime.Now;
            _activeGame.CurrentTurn = 999;
        }


        [Test]
        public void Is_Game_Created_Correctly()
        {
            //Assert
            Assert.That(_activeGame, Is.Not.EqualTo(null));
        }

        [Test]
        public void NextTurn_Adds_1_Turn()
        {
            // Arrange
            _activeGame.CurrentTurn = 0;
            // Act
            _activeGame.NextTurn();

            //Assert
            Assert.That(_activeGame.CurrentTurn, Is.EqualTo(1));
        }

        [TestCase(1)]
        [TestCase(5)]
        public void Adding_New_Players_Result_In_Correct_Number(int numberOfPlayersToAdd)
        {
            for (int i = 0; i < numberOfPlayersToAdd; i++)
            {
                Player player = new Player($"player{i}");

            }

            Assert.That(_activeGame.HumanPlayers.Count, Is.EqualTo(numberOfPlayersToAdd));
        }

        [Test]
        public void Destroy_Activegame_Results_in_an_empty_game()
        {
            // Act
            _activeGame.DestroyGameInstance();
            _activeGame = ActiveGameContainer.GetGameInstance;

            Assert.That(_activeGame.Name, Is.EqualTo("Empty Game"));
        }



        [Test]
        public void AllCharacters_Are_The_Sum_Of_Human_And_NPCs()
        {
            int noOfHumans = 2;
            int noOfNPCs = 6;

            _activeGame.HumanPlayers = new List<Player>() { };
            _activeGame.NonHumanPlayers = new List<Npc>() { };

            for (int i = 0; i < noOfHumans; i++)
            {
                Player player = new($"player{i}");
                _activeGame.HumanPlayers.Add(player);
            }

            for (int i = 0; i < noOfNPCs; i++)
            {
                Npc npc = new();
                _activeGame.NonHumanPlayers.Add(npc);
            }

            Assert.That(_activeGame.AllCharacters, Has.Count.EqualTo(noOfHumans + noOfNPCs));
        }
    }
}