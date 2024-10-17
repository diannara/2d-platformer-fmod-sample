using UnityEngine;

using TIGD.Audio.FMOD;
using TIGD.Audio.FMOD.Services;
using TIGD.Services;

namespace TIGD.Platformer.UI.Widgets
{
    public class ButtonEventHandler : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioCue _onButtonEnter;
        [SerializeField] private AudioCue _onButtonExit;
        [SerializeField] private AudioCue _onButtonClicked;

        public void OnButtonEnter()
        {
            if(_onButtonEnter == null)
            {
                return;
            }

            if(ServiceLocator.TryGet(out FMODAudioService audioService))
            {
                audioService.PlaySound(_onButtonEnter);
            }
        }

        public void OnButtonExit()
        {
            if(_onButtonExit == null)
            {
                return;
            }

            if(ServiceLocator.TryGet(out FMODAudioService audioService))
            {
                audioService.PlaySound(_onButtonExit);
            }
        }

        public void OnButtonClicked()
        {
            if(_onButtonClicked == null)
            {
                return;
            }

            if(ServiceLocator.TryGet(out FMODAudioService audioService))
            {
                audioService.PlaySound(_onButtonClicked);
            }
        }
    }
}
