using System.Collections.Generic;
using UnityEngine;

namespace Nemuri.Core
{
    public class PartyManager : MonoBehaviour
    {
        public static PartyManager Instance { get; private set; }

        [System.Serializable]
        public class PartyMember
        {
            public Transform transform;
            public Animator animator;
            public Rigidbody2D rb;
        }

        [Header("Party Setup")]
        [Tooltip("The first member is the leader. The rest will follow in order.")]
        public List<PartyMember> members = new List<PartyMember>();

        [Header("Follow Settings")]
        [Tooltip("The distance the leader must move before dropping a history point.")]
        public float pointSpacing = 0.1f;
        [Tooltip("How many points behind the leader the next member should follow. (Distance = pointSpacing * pointsBetweenMembers)")]
        public int pointsBetweenMembers = 12; 
        [Tooltip("Match this to your Player's move speed (default is 5)")]
        public float moveSpeed = 5f;

        private struct PointData
        {
            public Vector3 position;
            public Vector2 direction;
        }

        private List<PointData> _history = new List<PointData>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            if (members.Count > 0 && members[0].transform != null)
            {
                _history.Add(new PointData { 
                    position = members[0].transform.position, 
                    direction = Vector2.down
                });
            }
        }

        private void FixedUpdate()
        {
            if (members.Count <= 1 || members[0].transform == null) return;

            Transform leader = members[0].transform;
            Vector3 currentLeaderPos = leader.position;
            
            if (_history.Count == 0)
            {
                _history.Add(new PointData { position = currentLeaderPos, direction = Vector2.down });
            }

            Vector3 lastRecordedPos = _history[0].position;

            // 1. Record new history point if leader moved enough
            if (Vector3.Distance(currentLeaderPos, lastRecordedPos) >= pointSpacing)
            {
                Vector2 dir = (currentLeaderPos - lastRecordedPos).normalized;
                
                // Snap direction to 4-way for cleaner follower animations
                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) { dir.y = 0; dir.x = Mathf.Sign(dir.x); }
                else { dir.x = 0; dir.y = Mathf.Sign(dir.y); }

                _history.Insert(0, new PointData { 
                    position = currentLeaderPos, 
                    direction = dir
                });

                // Keep history trimmed to the maximum required length to save memory
                int maxHistory = (members.Count - 1) * pointsBetweenMembers + 1;
                if (_history.Count > maxHistory)
                {
                    _history.RemoveAt(_history.Count - 1);
                }
            }

            // 2. Move followers
            for (int i = 1; i < members.Count; i++)
            {
                PartyMember follower = members[i];
                if (follower.transform == null) continue;

                int targetIndex = i * pointsBetweenMembers;

                // Clamp to oldest available history if party hasn't moved enough yet
                if (targetIndex >= _history.Count)
                {
                    targetIndex = _history.Count - 1;
                }

                PointData targetData = _history[targetIndex];
                float dist = Vector3.Distance(follower.transform.position, targetData.position);
                
                if (dist > 0.05f) // Small threshold to prevent micro-jitter
                {
                    Vector3 newPos = Vector3.MoveTowards(follower.transform.position, targetData.position, moveSpeed * Time.fixedDeltaTime);
                    
                    if (follower.rb != null)
                    {
                        follower.rb.MovePosition(newPos);
                    }
                    else
                    {
                        follower.transform.position = newPos;
                    }

                    if (follower.animator != null)
                    {
                        follower.animator.SetBool("IsMoving", true);
                        follower.animator.SetFloat("MoveX", targetData.direction.x);
                        follower.animator.SetFloat("MoveY", targetData.direction.y);
                        follower.animator.SetFloat("LastMoveX", targetData.direction.x);
                        follower.animator.SetFloat("LastMoveY", targetData.direction.y);
                    }
                }
                else
                {
                    // Snap to position if very close to avoid micro-adjustments
                    if (follower.rb != null) follower.rb.MovePosition(targetData.position);
                    else follower.transform.position = targetData.position;

                    if (follower.animator != null)
                    {
                        follower.animator.SetBool("IsMoving", false);
                    }
                }
            }
        }
    }
}