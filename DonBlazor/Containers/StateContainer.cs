namespace DonBlazor.Containers
{
    public class StateContainer
    {
        private GameStatus? _gameStatus;

        public GameStatus GameStatus
        {
            get => _gameStatus ??= GameStatus.Unknown;
            set
            {
                _gameStatus = value;
                NotifyStateChanged();
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public enum GameStatus
    {
        Unknown,
        PreStart,
        Started,
        Finished
    }

}
