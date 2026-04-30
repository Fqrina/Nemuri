using UnityEngine;
using UnityEngine.InputSystem;

namespace Nemuri.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        
        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private PlayerInput _playerInput;
        private InputAction _moveAction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
            
            // Assuming the PlayerInput component is configured with the InputSystem_Actions
            // and has a "Move" action in the "Player" map.
            if (_playerInput != null)
            {
                _moveAction = _playerInput.actions["Move"];
            }
            
            // Set up Rigidbody2D for top-down movement
            _rb.gravityScale = 0f;
            _rb.freezeRotation = true;
        }

        private void Update()
        {
            if (_moveAction != null)
            {
                _moveInput = _moveAction.ReadValue<Vector2>();
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            _rb.MovePosition(_rb.position + _moveInput * _moveSpeed * Time.fixedDeltaTime);
        }
    }
}
