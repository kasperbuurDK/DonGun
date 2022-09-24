namespace SharedClassLibrary
{
    public class Cup
    {
        public readonly List<Dice> DiceList;
        private int _rolledCount = 0;

        public event EventHandler Rolled;

        public int MaxDice { get; set; } = 8;

        public Cup()
        {
            DiceList = new List<Dice>();
        }
        public Cup(int max)
            : this()
        {
            MaxDice = max;
        }

        public void Add(Dice e)
        {
            if (e != null)
            {
                if (DiceList.Count >= MaxDice)
                    DiceList.RemoveAt(DiceList.Count - 1);
                e.Rolled += OnRolledEventHandler;
                DiceList.Add(e);
            }
        }
        public Dice? Remove()
        {
            if (DiceList.Count > 0)
            {
                Dice res = DiceList.Last();
                DiceList.Remove(res);
                return res;
            }
            return null;
        }
        public void Clear()
        {
            DiceList.Clear();
        }
        public int RollCup()
        {
            _rolledCount = 0;
            foreach (Dice p in DiceList)
            {
                p.Roll();
            }
            return DiceList.Count;
        }

        void OnRolledEventHandler(object? sender, EventArgs e)
        {
            _rolledCount++;
            if (_rolledCount >= DiceList.Count)
            {
                Rolled.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
