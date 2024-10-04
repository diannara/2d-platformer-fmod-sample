using UnityEngine;

using TIGD.Audio.FMOD;

namespace TIGD.Collectables
{
    public abstract class AbstractCollectable : MonoBehaviour, ICollectable
    {
        [SerializeField] protected AudioCue _collectionAudio;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Check if the object colliding with the coin is the player
            if(collision.CompareTag("Player"))
            {
                // Collect the coin
                Collect();
            }
        }

        public abstract void Collect();
    }
}
