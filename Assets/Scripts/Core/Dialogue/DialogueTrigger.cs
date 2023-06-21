using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private bool hasTriggered;

    public void TriggerDialogue(DialogueManager dialogueManager)
    {
        if (dialogueManager == null)
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        else
            dialogueManager.StartDialogue(dialogue);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            hasTriggered = true;
        }
    }
}
