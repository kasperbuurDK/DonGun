namespace SharedClassLibrary.Actions
{
    public class OffensiveAction : IAnAction
    {
        public int ChanceToHit { get; set; }
        public bool MakeBasicAttack()
        {
            Random random = new Random();

            return ChanceToHit <= random.Next(0, 101);
        }
    }
}