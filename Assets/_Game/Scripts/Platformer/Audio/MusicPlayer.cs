using UnityEngine;

using FMOD.Studio;

using TIGD.Audio.FMOD;
using TIGD.Audio.FMOD.Services;
using TIGD.Services;

namespace TIGD.Platformer.Audio
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioCue _backgroundMusic;

        private EventInstance _backgroundMusicReference;

        private void Start()
        {
            if(_backgroundMusic == null)
            {
                Debug.LogWarning("MusicPlayer :: Start() :: Background music was not setup properly. AudioCue is null!");
                return;
            }

            if(ServiceLocator.TryGet(out FMODAudioService audioService))
            {
                audioService.PlayBackgroundMusic(_backgroundMusic);
            }
        }
    }
}
