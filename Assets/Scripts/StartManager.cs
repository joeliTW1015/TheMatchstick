using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


//TODO 把End的部分拿掉，獨立出一個EndManager
public class StartManager : MonoBehaviour
{
    [SerializeField] Animator canvasAnimator;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI contentText;
    [SerializeField] GameObject dialoguePanel, black;
    [SerializeField] float speakGap = 0.05f;
    [SerializeField] Dialogue dialogue;
    [SerializeField] bool isEndScene;
    AudioManager centerSound;
    RepeatDetector repeatDetector;
    Queue<string> sentences;
    bool isSpeaking, isEndPoint;

    private void Start() 
    {
        centerSound = FindObjectOfType<AudioManager>();
        repeatDetector = FindObjectOfType<RepeatDetector>();
        sentences = new Queue<string>();
        isSpeaking = false;
        StartCoroutine(InvalidateBlack());
        if(isEndScene)
        {
            centerSound.end.Play();
            Invoke("StartANewGame", 3);
        }
    }
    IEnumerator InvalidateBlack()
    {
        yield return new WaitForSecondsRealtime(2);
        black.SetActive(false);
    }
    public void StartANewGame()
    {
        PlayerPrefs.SetInt("FirstTimePlay", 0);
        InitializeArchive();
        black.SetActive(true);
        canvasAnimator.SetTrigger("fadeOut");
        StartCoroutine(StartDiaEnumerator());
    }

    public void ContinueTheGame()
    {
        if(PlayerPrefs.GetInt("FirstTimePlay", 1) == 1)
        {
            StartANewGame();
        }
        black.SetActive(true);
        canvasAnimator.SetTrigger("fadeOut");
        Invoke("LoadGame", 1.5f);
    }

    void InitializeArchive()
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex + 1);
        List<int> tempList = repeatDetector.triggeredDialogues;
        PlayerPrefs.SetString("TriggeredDialogues", JsonUtility.ToJson(new JSONableListWrapper<int>(tempList)));
        tempList = repeatDetector.pickedMatchBox;
        PlayerPrefs.SetString("PickedMatchBox", JsonUtility.ToJson(new JSONableListWrapper<int>(tempList)));

        //debug
        /*
        Debug.Log(repeatDetector.pickedMatchBox.Capacity);
        Debug.Log(repeatDetector.triggeredDialogues.Capacity);
        Debug.Log(PlayerPrefs.GetString("TriggeredDialogues", "Empty!"));
        Debug.Log(PlayerPrefs.GetString("PickedMatchBox", "Empty!"));
        */
        
        PlayerPrefs.SetInt("IsBeginingOfTheLevel", 1);
        PlayerPrefs.SetInt("MatchNumAtBeginOfTheLevel", 5);
    }
    IEnumerator StartDiaEnumerator()
    {
        yield return new WaitForSecondsRealtime(1f);
        StartDialogue(dialogue);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Start a conversation");
        dialoguePanel.SetActive(true);
        nameText.text = dialogue.name;
        isEndPoint = dialogue.isEndOfLevel;
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
            Time.timeScale = 1;
            
            if(isEndPoint)
            {
                if(isEndScene)
                    SceneManager.LoadScene(1); 
                else
                    LoadGame();
            }
            return;
        }
        contentText.text = " ";
        StartCoroutine("Speak");
    }

    void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level", SceneManager.GetActiveScene().buildIndex + 1));
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

    public void PlayButtonSound()
    {
        centerSound.button.Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
