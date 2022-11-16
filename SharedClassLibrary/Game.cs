using SharedClassLibrary;
using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary
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

        public List<Npc> NonHumanPlayers { get; set; } = new List<Npc>() { };

        public List<Character> AllCharacters
        {
            get
            {
                List<Character> listOfAll = new List<Character>();
                listOfAll.AddRange(HumanPlayers);
                listOfAll.AddRange(NonHumanPlayers);
                return listOfAll;
            } 

        }
        public int CurrentTurn { get; set; } = 0;

        public int CurrentPlayer { get; set; } = 0;

        public Character CharacterToAct { get; set; }

        public Game()
        {
            
 
        }


    }
}
