using SharedClassLibrary;
using System.ComponentModel.DataAnnotations;


/// Summary
/// 
/// Summary

namespace DonBlazor.Containers
{
    public sealed class ActiveGameContainer
    {

        private static ActiveGameContainer GameInstance = null;

        public static ActiveGameContainer GetGameInstance   // Singleton Pattern
        {
            get
            {
                GameInstance ??= new ActiveGameContainer();  // ?? is null-coalescing operator. ??= returns lhs if its not null else assigns rhs
                return GameInstance;
            }
        }

        private ActiveGameContainer() // Singleton Pattern
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

        public void SetCurrentPlayer()
        {
            CurrentCharacter = CurrentTurn % AllCharacters.Count;
        }



    }
}