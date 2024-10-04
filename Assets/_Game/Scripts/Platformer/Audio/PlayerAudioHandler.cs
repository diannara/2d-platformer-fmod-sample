using UnityEngine;

using FMODUnity;

using TIGD.Audio.FMOD;
using TIGD.Audio.FMOD.Services;
using TIGD.Platformer.Controllers;
using TIGD.Services;
using TIGD.Timers;

namespace TIGD.Platformer.Audio
{
    [RequireComponent(typeof(CharacterMotor))]
    public class PlayerAudioHandler : MonoBehaviour
    {
        [Header("Timers")]
        [SerializeField] private float _footstepInterval = 0.25f;

        [Header("Sound Emitters")]
        [SerializeField] private StudioEventEmitter _footstepEmitter;

        [Header("Audio")]
        [SerializeField] private AudioCue _footstepsAudio;
        [SerializeField] private AudioCue _jumpAudio;
        [SerializeField] private AudioCue _landingAudio;

        private CharacterMotor _motor;
        private CooldownTimer _footstepTimer;

        private void Awake()
        {
            _motor = GetComponent<CharacterMotor>();

            _footstepTimer = new CooldownTimer(_footstepInterval);
            _footstepTimer.OnTimerStop += () =>
            {
                PlayFootstepSfx();
                _footstepTimer.Reset(_footstepInterval);
                _footstepTimer.Start();
            };
            _footstepTimer.Start();
        }

        private void OnEnable()
        {
            _motor.OnJump += PlayJumpSfx;
            // _motor.OnLand += PlayLandingSfx;
        }

        private void OnDisable()
        {
            _motor.OnJump -= PlayJumpSfx;
            // _motor.OnLand -= PlayLandingSfx;
        }

        private void PlayFootstepSfx()
        {
            if(_footstepEmitter == null || _footstepsAudio == null)
            {
                return;
            }

            _footstepEmitter.SetParameter("Grass", 1.0f);
            _footstepEmitter.Play();
        }

        private void PlayJumpSfx()
        {
            if(_jumpAudio == null)
            {
                return;
            }

            if(ServiceLocator.TryGet(out FMODAudioService audioService))
            {
                audioService.PlaySound(_jumpAudio);
            }
        }

        private void PlayLandingSfx()
        {
            if(_landingAudio == null)
            {
                return;
            }

            if(ServiceLocator.TryGet(out FMODAudioService audioService))
            {
                audioService.PlaySoundWithParameter(_landingAudio, transform.position, "Grass", 1.0f);
            }
        }

        private void Start()
        {
            if(ServiceLocator.TryGet(out FMODAudioService audioService))
            {
                _footstepEmitter = audioService.InitalizeEventEmitter(_footstepEmitter, _footstepsAudio.EventReference);
            }
        }

        private void Update()
        {
            if(!_motor.IsGrounded)
            {
                return;
            }

            if(_motor.CurrentVelocity.x == 0)
            {
                return;
            }

            _footstepTimer.Tick(Time.deltaTime);
        }
    }
}
