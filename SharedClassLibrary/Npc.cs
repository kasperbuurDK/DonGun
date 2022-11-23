namespace SharedClassLibrary
{
    public class Npc : Character
    {

        public Npc()
        {

        }

        public Npc(NPCtype type, int number)
        {
            Number = number;
            Type = type;
            SetStatsAccordingToType(type);
        }

        private void SetStatsAccordingToType(NPCtype type)
        {
            
        }

        public NPCtype Type { get; set; }
        public int Number { get; set; }
    }
}
