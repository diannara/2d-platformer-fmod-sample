using UnityEngine;

using TIGD.Audio.FMOD.Services;
using TIGD.Services;

namespace TIGD.Audio.FMOD
{
    public class PlayAudioCue : MonoBehaviour
    {
        [SerializeField] private AudioCue _audioCue;

        public void Play()
        {
            if(ServiceLocator.TryGet(out FMODAudioService audioService))
            {
                audioService.PlaySound(_audioCue);
            }
        }
    }
}
