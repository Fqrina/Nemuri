using UnityEngine;

namespace Nemuri.Core
{
    public class TeleportPortal : MonoBehaviour
    {
        [Header("Teleport Settings")]
        [SerializeField] private Vector3 _targetPosition = new Vector3(100f, 0f, 0f);

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the object entering the trigger is the player
            // We check for the PlayerMovement component to be sure it's our player
            if (other.CompareTag("Player") || other.GetComponent<Nemuri.Player.PlayerMovement>() != null)
            {
                Teleport(other.transform);
            }
        }

        private void Teleport(Transform playerTransform)
        {
            // Move the transform directly
            playerTransform.position = _targetPosition;
            
            // If the player has a Rigidbody2D, we must update its position too
            // to prevent the physics engine from 'fighting' the transform change
            Rigidbody2D rb = playerTransform.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.position = _targetPosition;
                // Optional: Stop movement velocity upon teleporting
                rb.linearVelocity = Vector2.zero;
            }
            
            Debug.Log($"[TeleportPortal] Player teleported to: {_targetPosition}");
        }
    }
}
