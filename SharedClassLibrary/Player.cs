namespace SharedClassLibrary
{
    public class Player : Character
    {

        public Player(string name) 
        {
            Name = name;
        }

        public Player() { }

        public string OwnerName { get; set; }

        // TODO make it a more advanced not just string
        public string? RaceString { get; set; } 

        public string? MasteryString { get; set; }

        public string? StatusString { get; set; }

        
    }
}
