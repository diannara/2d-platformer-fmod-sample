using UnityEngine;

namespace TIGD.Platformer.Controllers
{
    public interface ICharacterController
    {
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime);
    }
}
