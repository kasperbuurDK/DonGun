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
            Name = type.ToString() + $" {number}";

            AdjustStatsAccordingToType(type);
        }

        private void AdjustStatsAccordingToType(NPCtype type)
        {
            if (type.ToString().StartsWith("Orc")) 
            {
                Strength += 3;
                Constitution += 2;
                Intelligence -= 5;
                Charisma -= 6;
                Dexterity -= 2;
            }
            else if ((type.ToString().StartsWith("Undead"))) 
            {
                Strength -= 4;
                Constitution += 2;
                Intelligence -= 8;
                Charisma -= 6;
                Wisdome -= 2;
            }

            if (type.ToString().EndsWith("Brute"))
            {
                Strength += 3;
                Constitution += 2;
                Intelligence -= 4;
                Charisma -= 2;
                Dexterity += 2;
                Wisdome -= 2;                    
            }
            else if (type.ToString().EndsWith("Mage")) 
            {
                Strength -= 3;
                Constitution -= 3;
                Intelligence += 4;
                Charisma += 2;
                Wisdome += 2;
            }


        }

        public int Number { get; set; }
        public NPCtype Type { get; set; }
       

        public enum NPCtype
        {
            Orc_Brute,
            Orc_Mage,
            Undead_Brute,
            Undead_Mage
        }
    }
}
