using SharedClassLibrary.AuxUtils;

namespace SharedClassLibrary.Actions
{
    public class OffensiveAction : IAnAction
    {      
        public int ChanceToSucced { get; set; }
        public string SenderSignature { get; set; }
        public string RecieverSignature { get; set; }
        public string Signature { get; init; }

        public OffensiveAction(string sender, string reciever)
        {
            Signature = Guid.NewGuid().ToString();
            SenderSignature = sender;
            RecieverSignature = reciever;
        }


        public bool MakeBasicAction(int diceValue, Character sender, Character reciever)
        {
            bool hit = DetermineIfHit(diceValue);
            if (hit)
            {
                reciever.RecieveDamage(sender.CalculateDamageGive(diceValue));
            }

            return hit;
        }

        private bool DetermineIfHit(int diceValue)
        {
            int diceModifier = 0;

            int precissionModifier = 100;
            if (diceValue == 10)
            {
                // dont change diceModifier
            }
            else if (diceValue < 10)
            {
                diceModifier = -(10 - diceValue);
            }
            else if (diceValue > 10)
            {
                diceModifier = diceValue - 10;
            }

            int chanceToHitAfterApplyingDice = ChanceToSucced + diceModifier;

            return chanceToHitAfterApplyingDice * precissionModifier > GameMasterHelpers.RandomRange(0, 101 * precissionModifier);
        }

        
    }
}