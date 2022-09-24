using System.ComponentModel;

namespace SharedClassLibrary
{
    public abstract class Dice
    {
        private int _max;
        private int _min;
        private const int _maxSleep = 500;
        private TimeSpan _timeSpan;

        public event EventHandler Rolled;
        public event EventHandler RollingChanged;

        private Random Rand { get; init; }
        private BackgroundWorker Worker { get; set; }
        public int Maximum
        {
            set
            {
                if (value >= _min)
                    _max = value;
                else
                    _max = _min + 1;
            }
            get { return _max; }
        }
        public int Minimum
        {
            set
            {
                if (value > _max)
                    _min = _max - 1;
                else
                    _min = value;
                Result = _min;
            }
            get { return _min; }
        }
        public int Result { get; private set; }

        public TimeSpan RollingDuration
        {
            set
            {
                if (value > TimeSpan.Zero)
                    _timeSpan = value;
                else
                    _timeSpan = new TimeSpan(0, 0, 0, 0, 300);
            }
            get { return _timeSpan; }
        }

        public Dice(int max, int min)
        {
            Maximum = max;
            Minimum = min;
            Rand = new Random();

            Worker = new BackgroundWorker { WorkerReportsProgress = true };
            Worker.DoWork += OnWorkerDoWork;
            Worker.ProgressChanged += OnWorkerProgressChanged;
            Worker.RunWorkerCompleted += OnWorkerRunWorkerCompleted;
        }

        public virtual void Roll()
        {
            if (!Worker.IsBusy)
            {
                Worker.RunWorkerAsync();
            }
        }

        private void OnWorkerDoWork(object? sender, DoWorkEventArgs e)
        {
            DateTime finishTime = DateTime.UtcNow + RollingDuration;
            float time, duration = (float)RollingDuration.TotalMilliseconds;
            int sleepDuration;

            while (finishTime > DateTime.UtcNow)
            {
                Result = Rand.Next(Minimum, Maximum + 1);
                time = (float)(finishTime - DateTime.UtcNow).TotalMilliseconds;
                sleepDuration = (int)((duration - time) * OutQuad(time.Remap(0, duration, 1, 0)) + 50);
                Worker.ReportProgress(0);
                Thread.Sleep(sleepDuration > _maxSleep ? _maxSleep : sleepDuration);
            }
        }
        public static float OutQuad(float x)
        {
            return (float)(1 - Math.Pow(1 - x, 3));
        }

        private void OnWorkerProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            RaiseEvent(RollingChanged);
        }

        private void OnWorkerRunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            RaiseEvent(Rolled);
        }

        private void RaiseEvent(EventHandler handler)
        {
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
