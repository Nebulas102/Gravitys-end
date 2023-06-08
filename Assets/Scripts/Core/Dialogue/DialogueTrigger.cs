using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private bool hasTriggered;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered)
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            hasTriggered = true;
        }
    }
}
