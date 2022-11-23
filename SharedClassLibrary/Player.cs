namespace SharedClassLibrary
{
    public class Player : Character
    {

        public Player(string name)
        {
            Name = name;
        }

        public Player() { }

        public string OwnerName { get; set; } = string.Empty;

        // TODO make it a more advanced not just string
        public string RaceString { get; set; } = string.Empty;

        public string MasteryString { get; set; } = string.Empty;

        public string StatusString { get; set; } = string.Empty;


    }
}
