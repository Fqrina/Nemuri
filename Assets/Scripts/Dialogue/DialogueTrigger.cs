using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Nemuri.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private TextAsset _dialogueJson;

        public void TriggerDialogue()
        {
            if (_dialogueJson == null)
            {
                Debug.LogWarning("No dialogue JSON assigned to this trigger.");
                return;
            }

            DialogueSequence sequence = JsonUtility.FromJson<DialogueSequence>(_dialogueJson.text);
            if (sequence != null && sequence.nodes != null)
            {
                DialogueManager.Instance.StartConversation(sequence.nodes);
            }
        }

        // For testing: trigger on start or when pressing a key
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) // Temporary debug key
            {
                TriggerDialogue();
            }
        }
    }
}
