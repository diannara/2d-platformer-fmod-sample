using System;

namespace TIGD.SceneManagement
{
    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> OnProgressChanged;

        const float ratio = 1.0f;

        public void Report(float value)
        {
            OnProgressChanged?.Invoke(value / ratio);
        }

        public void ClearProgressListeners()
        {
            OnProgressChanged = null;
        }
    }
}
