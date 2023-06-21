using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacterAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject inWorldCharacter;

    [SerializeField]
    private GameObject characterPrefab;

    [SerializeField]
    private GameObject characterAltPrefab;

    private bool triggered;
    private bool finished;

    private void OnTriggerEnter(Collider other)
    {
        if(!triggered && !finished)
        {
            GameObject obj = Instantiate(characterAltPrefab, inWorldCharacter.transform.parent);
            Destroy(inWorldCharacter);
            inWorldCharacter = obj;
            triggered = true;
        }
    }

    void Update()
    {
        if(triggered)
            if(!DialogueManager.instance.dialogueActive)
            {
                GameObject obj = Instantiate(characterPrefab, inWorldCharacter.transform.parent);
                Destroy(inWorldCharacter);
                inWorldCharacter = obj;
                finished = true;
                triggered = false;
            }

    }
}
