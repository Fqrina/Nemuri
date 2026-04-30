using UnityEngine;
using UnityEngine.InputSystem;

namespace Nemuri.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        
        private Rigidbody2D _rb;
        private Animator _animator;
        private Vector2 _moveInput;
        private Vector2 _lastMoveDirection = Vector2.down;
        private PlayerInput _playerInput;
        private InputAction _moveAction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
            
            if (_playerInput != null)
            {
                _moveAction = _playerInput.actions["Move"];
            }
            
            _rb.gravityScale = 0f;
            _rb.freezeRotation = true;
        }

        private void Update()
        {
            if (_moveAction != null)
            {
                _moveInput = _moveAction.ReadValue<Vector2>();
            }

            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            _rb.MovePosition(_rb.position + _moveInput * _moveSpeed * Time.fixedDeltaTime);
        }

        private void UpdateAnimation()
        {
            if (_moveInput != Vector2.zero)
            {
                _animator.SetBool("IsMoving", true);
                _animator.SetFloat("MoveX", _moveInput.x);
                _animator.SetFloat("MoveY", _moveInput.y);
                _lastMoveDirection = _moveInput;
            }
            else
            {
                _animator.SetBool("IsMoving", false);
            }

            _animator.SetFloat("LastMoveX", _lastMoveDirection.x);
            _animator.SetFloat("LastMoveY", _lastMoveDirection.y);
        }
    }
}
