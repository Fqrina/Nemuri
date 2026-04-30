using UnityEngine;
using UnityEngine.InputSystem;

namespace Nemuri.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement Instance { get; private set; }

        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        
        private Rigidbody2D _rb;
        private Animator _animator;
        private Vector2 _moveInput;
        private Vector2 _lastMoveDirection = Vector2.down;
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private bool _canMove = true;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _playerInput = GetComponent<PlayerInput>();
            
            _rb.gravityScale = 0f;
            _rb.freezeRotation = true;
        }

        private void OnEnable()
        {
            if (_playerInput != null)
            {
                // Ensure the "Player" map is active and the asset is enabled
                _playerInput.actions.Enable();
                _playerInput.SwitchCurrentActionMap("Player");
                _moveAction = _playerInput.currentActionMap.FindAction("Move");
                
                if (_moveAction != null)
                {
                    Debug.Log($"[PlayerMovement] Found 'Move' action in map '{_playerInput.currentActionMap.name}'.");
                }
            }
        }

        private void Update()
        {
            if (!_canMove)
            {
                _moveInput = Vector2.zero;
            }
            else if (_moveAction != null)
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

        public void SetCanMove(bool canMove)
        {
            _canMove = canMove;
            if (!canMove) _moveInput = Vector2.zero;
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
