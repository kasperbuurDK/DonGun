namespace DonBlazor.Containers
{
    public class StateContainer
    {
        private GameStatus? _gameStatus;
        private EncounterStatus? _encounterStatus;

        public GameStatus GameStatus
        {
            get => _gameStatus ??= GameStatus.Unknown;
            set
            {
                _gameStatus = value;
                NotifyStateChanged();
            }
        }

        public EncounterStatus EncounterStatus
        {
            get => _encounterStatus ??= EncounterStatus.Unknown;
            set
            {
                _encounterStatus = value;
                NotifyEncounterStatusChanged();
            }
        }




        public event Action? GameStatusChanged;
        public event Action? EncounterStatusChanged;

        private void NotifyStateChanged() => GameStatusChanged?.Invoke();

        public void NotifyEncounterStatusChanged() => EncounterStatusChanged?.Invoke();
    }

    public enum GameStatus
    {
        Unknown,
        PreStart,
        Started,
        Finished,
        NoRunningGame
    }

    public enum EncounterStatus
    {
        Unknown,
        NewTurnStarted,
        MidTurn,
        UsedAction,
        UsedMove,
        UsedMoveAndAction,
        EndingTurn
    }

}
