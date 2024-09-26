using UnityEngine;

namespace TIGD.Platformer.Controllers
{
    [DisallowMultipleComponent]
    public abstract class AbstractCharacterController : MonoBehaviour, ICharacterController
    {
        public abstract void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime);
    }
}
