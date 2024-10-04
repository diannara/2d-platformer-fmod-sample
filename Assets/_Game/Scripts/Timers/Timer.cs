using System;

namespace TIGD.Timers
{
    public abstract class Timer
    {
        public event Action OnTimerStart;
        public event Action OnTimerStop;

        protected float _currentTime;
        protected float _initialTime;

        public bool IsRunning { get; protected set; }
        public float CurrentTime => _currentTime;
        public float Progress => _currentTime / _initialTime;

        protected Timer(float initialTime)
        {
            _initialTime = initialTime;
            IsRunning = false;
        }

        public void Pause() => IsRunning = false;

        public void Resume() => IsRunning = true;

        public void Start()
        {
            _currentTime = _initialTime;
            if(!IsRunning)
            {
                IsRunning = true;
                OnTimerStart?.Invoke();
            }
        }

        public void Stop()
        {
            if(IsRunning)
            {
                IsRunning = false;
                OnTimerStop?.Invoke();
            }
        }

        public abstract void Tick(float deltaTime);
    }
}
