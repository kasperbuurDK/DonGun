namespace ServerSide.Models
{
    public class ARandomDude
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool IsAlive { get; set; } = true;

        public int Hp { get; set; } = 0;
        public int Rp { get; set; } = 0;
        public int Mp { get; set; } = 0;

    }
}
