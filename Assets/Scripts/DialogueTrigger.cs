using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public int number;
    public Dialogue dialogue;
    RepeatDetector repeatDetector;

    private void Start() 
    {
        repeatDetector = FindObjectOfType<RepeatDetector>();
        if(repeatDetector.triggeredDialogues[number] == 1)   
        {
            GameObject.Destroy(this.gameObject);
        }
        else if(repeatDetector.triggeredDialogues[number] == -1)
        {
            repeatDetector.triggeredDialogues[number] = 0;
        } 
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        repeatDetector.triggeredDialogues[number] = 1;
        GameObject.Destroy(this.gameObject);
    }

}
