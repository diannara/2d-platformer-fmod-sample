using TIGD.Audio.FMOD.Services;
using TIGD.Services;

namespace TIGD.Collectables
{
    public class Burger : AbstractCollectable
    {
        public override void Collect()
        {
            // TODO: Update health

            // if the audio cue is not null...
            if(_collectionAudio != null)
            {
                // Request the audio service...
                if(ServiceLocator.TryGet(out FMODAudioService audioService))
                {
                    // Play the sound!
                    audioService.PlaySound(_collectionAudio);
                }
            }

            // TODO: Play particle effect

            // Disable game object       
            this.gameObject.SetActive(false);
        }
    }
}
