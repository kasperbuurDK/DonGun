namespace DonSide.Models
{
    public class CurrentGame
    {
       // [Key]
       public int Id { get; set; }
       public string Name { get; set; }
       public int[] ActivePLayers { get; set; }
       public int CurrentTurn { get; set; }


        public CurrentGame()
        {

        }

    }
}
