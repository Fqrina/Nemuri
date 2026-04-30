using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Nemuri.Player;

namespace Nemuri.Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        [Header("UI References")]
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private Image _portraitImage;
        [SerializeField] private GameObject _continueIndicator;

        [Header("Settings")]
        [SerializeField] private float _defaultTypingSpeed = 0.05f;

        private PlayerInput _playerInput;
        private InputAction _interactAction;
        private Queue<DialogueNode> _nodes = new Queue<DialogueNode>();
        private bool _isTyping;
        private bool _waitingForInput;
        private DialogueNode _currentNode;
        private Coroutine _typingCoroutine;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            _dialoguePanel.SetActive(false);
        }

        private void Start()
        {
            // Try to find the Player's PlayerInput
            if (PlayerMovement.Instance != null)
            {
                _playerInput = PlayerMovement.Instance.GetComponent<PlayerInput>();
                SetupInput();
            }
        }

        private void SetupInput()
        {
            if (_playerInput != null)
            {
                _interactAction = _playerInput.actions.FindAction("Interact");
                if (_interactAction != null)
                {
                    _interactAction.performed += OnInteractAction;
                }
            }
        }

        private void OnDestroy()
        {
            if (_interactAction != null)
            {
                _interactAction.performed -= OnInteractAction;
            }
        }

        public void StartConversation(List<DialogueNode> sequence)
        {
            _nodes.Clear();
            foreach (var node in sequence)
            {
                _nodes.Enqueue(node);
            }

            if (PlayerMovement.Instance != null)
            {
                PlayerMovement.Instance.SetCanMove(false);
            }

            _dialoguePanel.SetActive(true);
            DisplayNextNode();
        }

        public void ShowDialogue(string speaker, string text)
        {
            var node = new DialogueNode { speaker = speaker, text = text, typingSpeed = _defaultTypingSpeed };
            StartConversation(new List<DialogueNode> { node });
        }

        public void DisplayNextNode()
        {
            if (_nodes.Count == 0)
            {
                EndConversation();
                return;
            }

            _currentNode = _nodes.Dequeue();
            _nameText.text = _currentNode.speaker;
            
            if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
            _typingCoroutine = StartCoroutine(TypeText(_currentNode.text, _currentNode.typingSpeed));
        }

        private IEnumerator TypeText(string text, float speed)
        {
            _isTyping = true;
            _dialogueText.text = text;
            _dialogueText.maxVisibleCharacters = 0;
            _continueIndicator.SetActive(false);

            for (int i = 0; i <= text.Length; i++)
            {
                _dialogueText.maxVisibleCharacters = i;
                yield return new WaitForSeconds(speed);
            }

            _isTyping = false;
            _waitingForInput = true;
            _continueIndicator.SetActive(true);
        }

        public void OnInteractAction(InputAction.CallbackContext context)
        {
            if (!_dialoguePanel.activeSelf) return;

            if (_isTyping)
            {
                StopCoroutine(_typingCoroutine);
                _dialogueText.maxVisibleCharacters = _currentNode.text.Length;
                _isTyping = false;
                _waitingForInput = true;
                _continueIndicator.SetActive(true);
            }
            else if (_waitingForInput)
            {
                _waitingForInput = false;
                DisplayNextNode();
            }
        }

        private void EndConversation()
        {
            _dialoguePanel.SetActive(false);
            if (PlayerMovement.Instance != null)
            {
                PlayerMovement.Instance.SetCanMove(true);
            }
            Debug.Log("Dialogue Ended");
        }
    }
}
