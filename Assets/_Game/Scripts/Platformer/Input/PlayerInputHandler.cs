using UnityEngine;
using UnityEngine.InputSystem;

using TIGD.Platformer.Controllers;

namespace TIGD.Platformer.Input
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerInputHandler : MonoBehaviour, InputMain.IPlayerActions
    {
        private InputMain _input;
        private PlayerController _controller;

        private Vector2 _moveVector;

        private bool _attackedThisFrame;
        private bool _interactedThisFrame;
        private bool _jumpedThisFrame;
        private bool _isAttackHeld;
        private bool _isInteractHeld;
        private bool _isJumpHeld;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();

            _input = new InputMain();
            _input.Player.SetCallbacks(this);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                _attackedThisFrame = true;
                _isAttackHeld = true;
            }
            else if(context.phase == InputActionPhase.Canceled)
            {
                _isAttackHeld = false;
            }
        }

        private void OnDisable()
        {
            _input.Player.Disable();
        }

        private void OnEnable()
        {
            _input.Player.Enable();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                _interactedThisFrame = true;
                _isInteractHeld = true;
            }
            else if(context.phase == InputActionPhase.Canceled)
            {
                _isInteractHeld = false;
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Performed)
            {
                _jumpedThisFrame = true;
                _isJumpHeld = true;
            }
            else if(context.phase == InputActionPhase.Canceled)
            {
                _isJumpHeld = false;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveVector = context.ReadValue<Vector2>();
        }

        public void Update()
        {
            PlayerInputs input = new PlayerInputs
            {
                MoveVector = _moveVector,
                AttackedThisFrame = _attackedThisFrame,
                InteractedThisFrame = _interactedThisFrame,
                JumpedThisFrame = _jumpedThisFrame,
                IsAttackHeld = _isAttackHeld,
                IsInteractHeld = _isInteractHeld,
                IsJumpHeld = _isJumpHeld,
            };
            
            // Reset the flags
            _attackedThisFrame = false;
            _interactedThisFrame = false;
            _jumpedThisFrame = false;

            _controller.SetInputs(ref input);
        }
    }
}
