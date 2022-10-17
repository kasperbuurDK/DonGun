namespace SharedClassLibrary
{
    public class Player : Character_abstract
    {
        public Player(string name)
        {
            this._name = name;
        }

        private string _name = "DJON DOE";

        public string Name { 
            set { 
              this._name = value;
            }
            get { return _name; } }


        // TODO make it a more advanced not just string
        public string? RaceString { get; set; } 

        public string? MasteryString { get; set; }

        public string? StatusString { get; set; }
    }
}
