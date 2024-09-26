using UnityEngine;

namespace TIGD.Platformer.Controllers
{
    [RequireComponent(typeof(CharacterMotor))]
    public class PlayerController : AbstractCharacterController
    {
        [Header("Movement")]
        [SerializeField] private float _maxSpeed = 14.0f;
        [SerializeField] private float _acceleration = 120.0f;
        [SerializeField] private float _groundDeceleration = 60.0f;
        [SerializeField] private float _airDeceleration = 30.0f;

        private CharacterMotor _motor;
        
        private float _jumpTimeCounter;

        private bool _isGrounded;
        private bool _isFacingRight;

        private Vector2 _moveVector;

        private void Awake()
        {
            _motor = GetComponent<CharacterMotor>();

            _isFacingRight = false;
        }

        public void SetInputs(ref PlayerInputs inputs)
        {
            _moveVector = inputs.MoveVector;

            if(inputs.JumpedThisFrame)
            {
                _motor.Jump();
            }
            else if(!inputs.JumpedThisFrame && !inputs.IsJumpHeld)
            {
                _motor.StopJumping();
            }
        }

        public override void UpdateVelocity(ref Vector3 velocity, float deltaTime)
        {
            if(_moveVector.x == 0.0f)
            {
                float deceleration = _motor.IsGrounded ? _groundDeceleration : _airDeceleration;
                velocity.x = Mathf.MoveTowards(velocity.x, 0.0f, deceleration * deltaTime);
            }
            else
            {
                velocity.x = Mathf.MoveTowards(velocity.x, _moveVector.x * _maxSpeed, _acceleration * deltaTime);
            }
        }
    }
}
