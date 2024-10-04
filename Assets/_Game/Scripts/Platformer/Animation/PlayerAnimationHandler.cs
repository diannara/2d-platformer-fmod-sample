using UnityEngine;

using TIGD.Platformer.Controllers;

namespace TIGD.Platformer.Animation
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int SpeedParameter = Animator.StringToHash("Speed");

        [Header("References")]
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void HandleSpriteFacing(float x)
        {
            if(x > 0.0f)
            {
                _spriteRenderer.flipX = true;
            }
            else if(x < 0.0f)
            {
                _spriteRenderer.flipX = false;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if(_animator == null)
            {
                _animator = GetComponentInChildren<Animator>();
            }

            if(_spriteRenderer == null)
            {
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }
#endif

        public void SetInputs(ref PlayerInputs input)
        {
            HandleSpriteFacing(input.MoveVector.x);

            _animator.SetFloat(SpeedParameter, input.MoveVector.magnitude);
            _animator.SetBool(IsJumping, input.IsJumpHeld);
        }
    }
}