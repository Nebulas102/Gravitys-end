using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public float textSpeed;

    public Animator animator;

    private Queue<string> sentences;

    private bool textIsTyping;
    private string currentSentence;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string senctence in dialogue.sentences)
        {
            sentences.Enqueue(senctence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(textIsTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            textIsTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSenctence());
    }

    IEnumerator TypeSenctence ()
    {
        textIsTyping = true;
        dialogueText.text = "";
        foreach (char letter in currentSentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        textIsTyping = false;
    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}
