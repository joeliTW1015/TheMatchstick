using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource wind, music, button, die, speak, end, heartBeat, whisper;
    private void Start() 
    {
        DontDestroyOnLoad(this.gameObject);
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void Update() {
        if(SceneManager.GetActiveScene().buildIndex == 4 && wind.enabled)
        {
            wind.enabled = false;
        }
        else if(SceneManager.GetActiveScene().buildIndex != 4 && !wind.enabled)
        {
            wind.enabled = true;
            wind.Play();
        }
    }
}
