using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI contentText;
    [SerializeField] GameObject dialoguePanel;
    GameObject player;
    [SerializeField]GameManager gameManager;
    [SerializeField] float speakGap = 0.05f;
    AudioManager centerSound;
    Queue<string> sentences;
    bool isSpeaking, isEndPoint;
    private void Start() 
    {
        centerSound = FindObjectOfType<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        sentences = new Queue<string>();
        isSpeaking = false;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isEndPoint = dialogue.isEndOfLevel;
        player.GetComponent<PlayerMove>().enabled = false;
        player.GetComponent<PlayerLight>().enabled = false;
        Debug.Log("Start a conversation");
        dialoguePanel.SetActive(true);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(isSpeaking)
        {
            return;
        }
        if(sentences.Count == 0)
        {
            dialoguePanel.SetActive(false);
            player.GetComponent<PlayerMove>().enabled = true;
            player.GetComponent<PlayerLight>().enabled = true;
            Time.timeScale = 1;
            
            if(isEndPoint)
            {
                gameManager.NextLevel();
            }
            return;
        }
        contentText.text = " ";
        StartCoroutine("Speak");
    }

    IEnumerator Speak()
    {
        isSpeaking = true;
        foreach(char letter in sentences.Dequeue())
        {
            contentText.text += letter;
            if(!centerSound.speak.isPlaying)
                centerSound.speak.Play();
            yield return new WaitForSecondsRealtime(speakGap);
        }
        isSpeaking = false;
    }

    public void SkipDialogue()
    {
        sentences.Clear();
        DisplayNextSentence();
    }

    
}
