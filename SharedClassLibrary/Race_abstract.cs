
namespace SharedClassLibrary
{
    public class Race_abstract
    {
        // Main stat amplifiers
        // Main meter amplifiers

        public RaceType Type { set; get; } = RaceType.Elf;

        public Race_abstract()
        { }

        public Race_abstract(int type) 
        {
            Type = (RaceType)type;
        }

        public Race_abstract(RaceType type)
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
