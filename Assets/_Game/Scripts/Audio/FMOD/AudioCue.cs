using UnityEngine;

using FMODUnity;

namespace TIGD.Audio.FMOD
{
    [CreateAssetMenu(fileName = "AudioCue", menuName = "TIGD/Audio/AudioCue")]
    public class AudioCue : ScriptableObject
    {
        public string DisplayName;
        [TextArea] public string Description;

        [Header("FMOD")]
        public EventReference EventReference;
    }
}
