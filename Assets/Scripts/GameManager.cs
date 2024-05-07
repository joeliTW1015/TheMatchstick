using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool debugLight;
    [SerializeField] Animator canvasAnimator;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] GameObject tempLight, pauseMenu;
    [SerializeField] TextMeshProUGUI matchNumText;
    [SerializeField] Vector3 initPlayerPos;
     [SerializeField] Dialogue dieDialogue, startDialogue;
    GameObject player;
    PlayerLight playerLight;
    AudioManager centerSound;
    RepeatDetector repeatDetector;
    string matchBoxPicked;
    private void Awake() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerLight = player.GetComponent<PlayerLight>();
        centerSound = FindObjectOfType<AudioManager>();
        repeatDetector = FindObjectOfType<RepeatDetector>();

        if(!debugLight)
            GameObject.Destroy(tempLight);

        if(PlayerPrefs.GetInt("IsBeginingOfTheLevel") == 1)
        {
            for(int i = 0; i < 20; i ++)
            {
                repeatDetector.triggeredDialogues[i] = -1;
                repeatDetector.pickedMatchBox[i] = -1;
            }
            playerLight.matchNum = PlayerPrefs.GetInt("MatchNumAtBeginOfTheLevel", -1);
        }
        else
        {
            string tempString = PlayerPrefs.GetString("PickedMatchBox");
            repeatDetector.pickedMatchBox = JsonUtility.FromJson< JSONableListWrapper<int> >(tempString).list;
            tempString = PlayerPrefs.GetString("TriggeredDialogues");
            repeatDetector.triggeredDialogues = JsonUtility.FromJson< JSONableListWrapper<int> >(tempString).list;
            playerLight.matchNum = PlayerPrefs.GetInt("MatchNum", -1);
        }
        
        //Debug
        Debug.Log(repeatDetector.pickedMatchBox.Capacity);
        Debug.Log(repeatDetector.triggeredDialogues.Capacity);
    }
    void Start()
    {
        Time.timeScale = 1;
        if(PlayerPrefs.GetInt("IsBeginingOfTheLevel") == 1)
        {
            player.transform.position = initPlayerPos;
            SaveGame(false);
            player.GetComponent<PlayerMove>().enabled = false;
            player.GetComponent<PlayerLight>().enabled = false;
            StartCoroutine(InitDialogue());
        }
        else
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX", 0), PlayerPrefs.GetFloat("PlayerPosY", 0));
    }

    IEnumerator InitDialogue()
    {
        yield return new WaitForSeconds(1);
        dialogueManager.StartDialogue(startDialogue);
    }

    // Update is called once per frame
    void Update()
    {
        //Temp
        if(Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
        if(Input.GetKeyDown(KeyCode.N))
        {
            NextLevel();
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene(1);
        }

        matchNumText.text = "กั " + playerLight.matchNum.ToString("00");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        StartCoroutine(RestartLevelIEnumerator());
    }

    public void RestartFromSavePoint()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        StartCoroutine(RestartFromSavePointIEnumerator());
    }

    public void NextLevel()
    {
        SaveGame(true);
        StartCoroutine(NextLevelIEnumerator());
    }

    IEnumerator RestartLevelIEnumerator()
    {
        canvasAnimator.SetTrigger("fadeOut");
        yield return new WaitForSeconds(2);
        PlayerPrefs.SetInt("IsBeginingOfTheLevel", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator RestartFromSavePointIEnumerator()
    {
        canvasAnimator.SetTrigger("fadeOut");
        yield return new WaitForSeconds(2);
        if(PlayerDie.isDead)
        {
            Time.timeScale = 0;
            dialogueManager.StartDialogue(dieDialogue);
            yield return new WaitForSeconds(0.4f);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator NextLevelIEnumerator()
    {
        canvasAnimator.SetTrigger("fadeOut");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayButtonSound()
    {
        centerSound.button.Play();
    }

    public void SaveGame(bool _isBeginingOfTheLevel)
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);

        PlayerPrefs.SetInt("IsBeginingOfTheLevel", _isBeginingOfTheLevel ? 1 : 0);
        if(_isBeginingOfTheLevel)
        {
            PlayerPrefs.SetInt("MatchNumAtBeginOfTheLevel", playerLight.matchNum);
        }

        List<int> tempList = repeatDetector.triggeredDialogues;
        PlayerPrefs.SetString("TriggeredDialogues", JsonUtility.ToJson(new JSONableListWrapper<int>(tempList)));
        tempList = repeatDetector.pickedMatchBox;
        PlayerPrefs.SetString("PickedMatchBox", JsonUtility.ToJson(new JSONableListWrapper<int>(tempList)));
        
        PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
        PlayerPrefs.SetInt("MatchNum", playerLight.matchNum);
        //Temp
        PlayerPrefs.SetInt("DialogueNum", -1);
    }

    public void PauseButtonPressed()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}

