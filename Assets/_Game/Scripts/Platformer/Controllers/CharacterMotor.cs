using UnityEngine;

namespace TIGD.Platformer.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(ICharacterController))]
    public class CharacterMotor : MonoBehaviour
    {
        [Header("Physics")]
        [SerializeField] private LayerMask _collisionLayer;
        [SerializeField] private float _gravity = 9.81f;
        [SerializeField] private float _groundCheckDistance = 0.05f;
        [SerializeField] private float _groundingForce = -1.5f;

        [Header("Jumping")]
        [SerializeField] private float _jumpForce = 36.0f;
        [SerializeField] private float _jumpBuffer = 0.2f;
        [SerializeField] private float _coyoteTime = 0.15f;
        [SerializeField] private float _fallAcceleration = 110.0f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3.0f;

        [Header("Debug")]
        [SerializeField] private bool _showDebug;

        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _collider;
        private ICharacterController _controller;

        private Vector3 _currentVelocity;

        private bool _canUseBufferedJump;
        private bool _canUseCoyoteTime;
        private bool _endedJumpEarly;
        private bool _isGrounded;
        private bool _isJumpHeld;
        private bool _jumpToConsume;
        private bool _queriesStartInColliders;

        private float _time;
        private float _timeJumpedWasPressed;
        private float _timeSinceGrounded = float.MinValue;

        public bool IsGrounded => _isGrounded;
        public bool ShowDebugInfo => _showDebug;

        private bool CanUseCoyoteTime => _canUseCoyoteTime && !_isGrounded && _time < _timeSinceGrounded + _coyoteTime;
        private bool HasBufferedJump => _canUseBufferedJump && _jumpToConsume && _time < _timeJumpedWasPressed + _jumpBuffer;

        public Vector3 CurrentVelocity => _currentVelocity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
            _controller = GetComponent<ICharacterController>();

            _queriesStartInColliders = Physics2D.queriesStartInColliders;
        }

        private void ApplyFinalVelocity() => _rigidbody.linearVelocity = _currentVelocity;

        private void CheckForCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            bool hitCeiling = Physics2D.CapsuleCast(_collider.bounds.center, _collider.size, _collider.direction, 0, Vector2.up, _groundCheckDistance, _collisionLayer);
            if(hitCeiling)
            {
                _currentVelocity.y = Mathf.Min(0.0f, _currentVelocity.y);
            }

            bool hitGround = Physics2D.CapsuleCast(_collider.bounds.center, _collider.size, _collider.direction, 0, Vector2.down, _groundCheckDistance, _collisionLayer);
            if(!_isGrounded && hitGround)
            {
                _isGrounded = true;
                _canUseCoyoteTime = true;
                _canUseBufferedJump = true;
                _endedJumpEarly = false;
            }
            else if(_isGrounded && !hitGround)
            {
                _isGrounded = false;
                _timeSinceGrounded = _time;
            }

            Physics2D.queriesStartInColliders = _queriesStartInColliders;
        }

        private void ExecuteJump()
        {
            _timeJumpedWasPressed = 0;

            _endedJumpEarly = false;

            _canUseBufferedJump = false;
            _canUseCoyoteTime = false;

            _currentVelocity.y = _jumpForce;
        }

        private void FixedUpdate()
        {
            CheckForCollisions();

            HandleJump();
            HandleMovement();
            HandleGravity();

            ApplyFinalVelocity();
        }

        private void HandleJump()
        {
            if(!_endedJumpEarly && !_isGrounded && !_isJumpHeld && _rigidbody.linearVelocity.y > 0.0f)
            {
                _endedJumpEarly = true;
            }

            if(!_jumpToConsume || !HasBufferedJump)
            {
                return;
            }

            if(_isGrounded || CanUseCoyoteTime)
            {
                ExecuteJump();
            }

            _jumpToConsume = false;
        }

        private void HandleGravity()
        {
            if(_isGrounded && _currentVelocity.y <= 0.0f)
            {
                _currentVelocity.y = _groundingForce;
            }
            else
            {
                float inAirGravity = _fallAcceleration;
                if(_endedJumpEarly && _currentVelocity.y > 0.0f)
                {
                    inAirGravity *= _jumpEndEarlyGravityModifier;
                }
                _currentVelocity.y = Mathf.MoveTowards(_currentVelocity.y, -_gravity, inAirGravity * Time.fixedDeltaTime);
            }
        }

        private void HandleMovement()
        {
            _controller.UpdateVelocity(ref _currentVelocity, Time.fixedDeltaTime);
        }

        public void Jump()
        {
            _isJumpHeld = true;
            _jumpToConsume = true;
            _timeJumpedWasPressed = _time;
        }

        public void StopJumping()
        {
            _isJumpHeld = false;
        }

        private void Update()
        {
            _time += Time.deltaTime;
        }
    }
}
