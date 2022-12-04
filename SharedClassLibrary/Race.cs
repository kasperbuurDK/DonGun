
namespace SharedClassLibrary
{
    public class Race
    {
        // Main stat amplifiers
        // Main meter amplifiers

        public RaceType Type { set; get; } = RaceType.Elf;

        public Race()
        { }

        public Race(int type)
        {
            Type = (RaceType)type;
        }

        public Race(RaceType type)
        {
            Type = type;
        }

        public enum RaceType
        {
            Elf,
            Human,
            Orc,
            Goblin,
            Troll,
            Direwolf,
            Dragon
        }
    }
}
