namespace TIGD.Timers
{
    public class CooldownTimer : Timer
    {
        public bool IsFinished => _currentTime <= 0;

        public CooldownTimer(float value) : base(value)
        {
        }

        public void Reset() => _currentTime = _initialTime;

        public void Reset(float newTime)
        {
            _initialTime = newTime;
            Reset();
        }

        public override void Tick(float deltaTime)
        {
            if(IsRunning && _currentTime > 0)
            {
                _currentTime -= deltaTime;
            }

            if(IsRunning && _currentTime <= 0)
            {
                Stop();
            }
        }
    }
}