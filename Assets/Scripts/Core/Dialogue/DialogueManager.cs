using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    // Singleton for DialogueManager
    public static DialogueManager Instance;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public float textSpeed;

    public Animator animator;

    private Queue<string> sentences;

    public bool dialogueActive;
    private bool textIsTyping;
    private string currentSentence;
    private InputManager _inputManager;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _inputManager = new InputManager();

        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (dialogueActive)
        {
            _inputManager.UI.Enable();
            if (_inputManager.UI.DisplayNextSentence.triggered)
                OnDisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueActive = true;
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string senctence in dialogue.sentences)
        {
            sentences.Enqueue(senctence);
        }

        OnDisplayNextSentence();
    }

    public void OnDisplayNextSentence()
    {
        if (textIsTyping)
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

    public IEnumerator TypeSenctence()
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

    public void EndDialogue()
    {
        dialogueActive = false;
        animator.SetBool("IsOpen", false);
    }
}
