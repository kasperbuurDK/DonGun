namespace DonBlazor.Models
{
    internal class Map
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Level { get; set; }

        public List<Fields>? playArea { get; set; }

    }
}