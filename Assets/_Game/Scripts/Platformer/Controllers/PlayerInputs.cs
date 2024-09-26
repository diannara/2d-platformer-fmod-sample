using UnityEngine;

namespace TIGD.Platformer.Controllers
{
    public struct PlayerInputs
    {
        public bool AttackedThisFrame;
        public bool InteractedThisFrame;
        public bool JumpedThisFrame;
        public bool IsAttackHeld;
        public bool IsInteractHeld;
        public bool IsJumpHeld;

        public Vector2 MoveVector;
    }
}
