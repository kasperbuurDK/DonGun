using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary.AuxUtils
{
    public class GameMasterHelpers
    {
        public static float DetermineDistanceBetweenCharacters(Character character, Character otherCharacter)
        {
            float deltaX =
                Math.Max(character.Position.X, otherCharacter.Position.X) - Math.Min(character.Position.X, otherCharacter.Position.X);
            float deltaY =
                Math.Max(character.Position.Y, otherCharacter.Position.Y) - Math.Min(character.Position.Y, otherCharacter.Position.Y);

            if (deltaX == 0) return deltaY;
            if (deltaY == 0) return deltaX;

            float distanceBetween = (float)Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            return distanceBetween;
        }

        public static int RandomRange(int minRange, int maxRange) 
        {
            Random random = new Random();

            return random.Next(minRange, maxRange);
        }
    }
}