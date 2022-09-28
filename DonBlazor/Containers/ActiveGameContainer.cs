using DonBlazor.Models;
using SharedClassLibrary;
using System.ComponentModel.DataAnnotations;


namespace DonBlazor.Containers
{
  
    public sealed class ActiveGameContainer
    {
        private static ActiveGameContainer GameInstance = null;

        /// <summary>
        /// Singleton creater of ActiveGameContainer
        /// </summary>
        public static ActiveGameContainer GetGameInstance   
        {
            get
            {
                GameInstance ??= new ActiveGameContainer();  // ?? is null-coalescing operator. ??= returns lhs if its not null else assigns rhs
                return GameInstance;
            }
        }

        private ActiveGameContainer() 
        {

        }

        // Properties      
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Should be created and assigned at server

        public string Name { get; set; } = "A Default DonGun Game";

        public List<Player> HumanPlayers { get; set; } = new List<Player>() { };

        public List<Character_abstract> AllCharacters { get; set; } = new List<Character_abstract> { };

        public int CurrentTurn { get; set; } = 0;

        public int CurrentCharacter{ get; set; } = 0;

        // Methods

        public void NextTurn()
        {
            CurrentTurn++;
        }

        public void SetCurrentCharacter()
        {
            CurrentCharacter = CurrentTurn % AllCharacters.Count;
        }

        public void AddPlayerToGame(Player newPlayer)
        {
            HumanPlayers.Add(newPlayer);
        }



    }
}