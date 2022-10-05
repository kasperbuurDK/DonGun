using SharedClassLibrary;
using System.ComponentModel.DataAnnotations;

namespace DonBlazor.Models
{
    public class Game
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString(); // Should be created and assigned at server

        [Required]
        public string Name { get; set; } = "Empty Game";

        public DateTime? LastSaved { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public List<Player> HumanPlayers { get; set; } = new List<Player>() { };

        public List<Character_abstract> AllCharacters { get; set; } = new List<Character_abstract> { };

        public int CurrentTurn { get; set; } = 0;

        public int CurrentPlayer { get; set; } = 0;

        private Map? _map;

        public Game()
        {
            
 
        }


    }
}
