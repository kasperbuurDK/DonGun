﻿using SharedClassLibrary.AuxUtils;

namespace SharedClassLibrary.Actions
{
    public class OffensiveAction : IAnAction
    {
                
        public int ChanceToSucced { get; set; }
        public Character Reciever { get; set; }
        public Character Sender { get; set; }

        public bool MakeBasicAction(int diceValue)
        {
            bool hit = DetermineIfHit(diceValue);
            if (hit)
            {
                Reciever.RecieveDamage(Sender.CalculateDamageGive(diceValue));
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