using System.Collections.Generic;

using UnityEngine;

using FMODUnity;
using FMOD.Studio;

// Required because TIGD.Audio.FMOD namespace conflict.
using FMODStudio = FMOD.Studio;

using TIGD.Services;

namespace TIGD.Audio.FMOD.Services
{
    public class FMODAudioService : AbstractService
    {
        private const string MUSIC_TRACK_PARAMETER = "Music";

        private List<EventInstance> _eventInstances = new List<EventInstance>();
        private List<StudioEventEmitter> _eventEmitters = new List<StudioEventEmitter>();

        private EventInstance _backgroundMusicReference;

        // Create event instance with event reference.
        public EventInstance CreateEventInstance(EventReference eventReference)
        {
            EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
            _eventInstances.Add(eventInstance);
            return eventInstance;
        }

        // Initalize event emitter with event reference.
        public StudioEventEmitter InitalizeEventEmitter(StudioEventEmitter emitter, EventReference eventReference)
        {
            if(emitter == null)
            {
                return null;
            }

            emitter.EventReference = eventReference;

            if(_eventEmitters.Contains(emitter))
            {
                return emitter;
            }

            _eventEmitters.Add(emitter);
            return emitter;
        }

        // Play one shot sound at a position.
        public void PlaySound(AudioCue cue, Vector3 position = default)
        {
            RuntimeManager.PlayOneShot(cue.EventReference, position);
        }

        public void PlayBackgroundMusic(AudioCue cue)
        {
            _backgroundMusicReference = CreateEventInstance(cue.EventReference);
            _backgroundMusicReference.start();
        }

        public void PlaySoundWithParameter(AudioCue cue, Vector3 position, string parameterName, float parameterValue)
        {
            EventInstance eventInstance = CreateEventInstance(cue.EventReference);
            eventInstance.setParameterByName(parameterName, parameterValue);
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
            eventInstance.start();
            eventInstance.release();
        }

        // Called when the service is shutdown.
        public override void Shutdown()
        {
            base.Shutdown();

            // Stop all event instances.
            foreach(EventInstance eventInstance in _eventInstances)
            {
                // eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                // Changed to FMODStudio.STOP_MODE.IMMEDIATE to fix namespace conflict.
                eventInstance.stop(FMODStudio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }

            _backgroundMusicReference.stop(FMODStudio.STOP_MODE.IMMEDIATE);
            _backgroundMusicReference.release();

            // Stop all event emitters.
            foreach(StudioEventEmitter emitter in _eventEmitters)
            {
                emitter.Stop();
            }
        }

        public void SwitchBackgroundMusicTrack(int index)
        {
            _backgroundMusicReference.setParameterByName(MUSIC_TRACK_PARAMETER, index);
        }
    }
}
