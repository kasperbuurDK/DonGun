using SharedClassLibrary;
using System.ComponentModel.DataAnnotations;

namespace DonBlazor.Containers
{
    public static class ActiveGameContainer
    {

        // Properties      
        public static string Id { get; set; } = Guid.NewGuid().ToString(); // Should be created and assigned at server

        public static string Name { get; set; } = "A Default DonGun Game";

        public static List<Player> HumanPlayers { get; set; } = new List<Player>() { };

        public static List<Character_abstract> AllCharacters { get; set; } = new List<Character_abstract> { };

        public static int CurrentTurn { get; set; } = 0;

        public static int CurrentCharacter{ get; set; } = 0;

        // Methods

        public static void NextTurn()
        {
            CurrentTurn++;
        }

        public static void SetCurrentPlayer()
        {
            CurrentCharacter = CurrentTurn % AllCharacters.Count;
        }



    }
}