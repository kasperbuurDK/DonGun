using DonBlazor.Client;
using SharedClassLibrary;

namespace DonGunTest
{
    internal class QueueTest
    {

        Player _player1;
        Player _player2;
        Player _player3;
        Npc _npc1;
        Npc _npc2;
        Npc _npc3;
        GameMaster _gameMaster;
        
        [SetUp]
        public void SetUp()
        {
            _gameMaster = new GameMaster(new Game());
            _player1 = new Player("Player1");
            _player2 = new Player("Player2");
            _player3 = new Player("Player3");


            _npc1 = new Npc();
            _npc2 = new Npc();
            _npc3 = new Npc();

            _gameMaster.AddCharacterToGame(_player1);
            _gameMaster.AddCharacterToGame(_player2);
            _gameMaster.AddCharacterToGame(_player3);

            _gameMaster.AddCharacterToGame(_npc1);
            _gameMaster.AddCharacterToGame(_npc2);
            _gameMaster.AddCharacterToGame(_npc3);

        }


        [Test]
        public void Queue_lengt_match_lengt_og_all_characters()
        {

            _gameMaster.StartEncounter();

            Assert.That(_gameMaster.Queue.Count, Is.EqualTo(6));
        }

        [Test]
        public void GameMaster_can_select_the_right_character_from_queue_at_start()
        {
            _gameMaster.StartEncounter();
            Assert.That(_gameMaster.Game.CharacterToAct, Is.EqualTo(_player1));
        }

        [Test]
        public void GameMaster_can_select_the_right_character_from_queue_after_turn_has_ended()
        {
            _gameMaster.StartEncounter();
            _gameMaster.EndTurn();

            Assert.That(_gameMaster.Game.CharacterToAct, Is.EqualTo(_player2));
        }
    }
}
