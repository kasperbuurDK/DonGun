namespace SharedClassLibrary
{
    public class Player : Character
    {
        private string _name = "DJON DOE";

        public string Name
        {
            set
            {
                SetPropertyField(nameof(Name), ref _name, value); ;
            }
            get { return _name; }
        }

        public Player(string name)
        {
            Name = name;
        }

        public Player() {}

        // TODO make it a more advanced not just string
        public string? RaceString { get; set; } 

        public string? MasteryString { get; set; }

        public string? StatusString { get; set; }
    }
}
