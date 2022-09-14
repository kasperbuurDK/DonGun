using System;

namespace SharedClassLibrary
{
    public abstract class Race_abstract
    {
        private RaceType _type;
        // Main stat amplifiers
        // Main meter amplifiers

        public RaceType Type { set; get; }

        public Race_abstract()
        {
        }

        enum RaceType
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
