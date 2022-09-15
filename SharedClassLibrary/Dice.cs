using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassLibrary
{
    public abstract class Dice
    {
        private int _max;
        private int _min;
        private TimeSpan _timeSpan;

        public event EventHandler Rolled;
        public event EventHandler RollingChanged;

        private Random Rand { get; init; }
        private BackgroundWorker Worker { get; set; }
        public int Maximum
        {
            set {  
                    if (value >= _min)
                        _max = value;
                    else
                        _max = _min+1;
                }
            get { return _max; }
        }
        public int Minimum
        {
            set
            {
                if (value > _max)
                    _min = _max-1;
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
                    _timeSpan = new TimeSpan(0,0,0,0,300);
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

        public void Roll()
        {
            if (!Worker.IsBusy)
            {
                Worker.RunWorkerAsync();
            }
        }

        private void OnWorkerDoWork(object? sender, DoWorkEventArgs e)
        {
            DateTime finishTime = DateTime.UtcNow + RollingDuration;
            float easeValue = (float)RollingDuration.TotalMilliseconds;

            while (finishTime > DateTime.UtcNow)
            {
                Result = Rand.Next(Minimum, Maximum + 1);
                Worker.ReportProgress(0);
                Thread.Sleep(25);
            }
        }
        public static float InQuad(float t) => t * t;
        public static float OutQuad(float t) => 1 - InQuad(1 - t);

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
