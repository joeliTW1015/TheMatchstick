using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerHallucination : MonoBehaviour
{
    PlayerDie playerDie;
    [SerializeField] GameObject redEye;
    [SerializeField] Material blurMaterial;
    [SerializeField] float heartBeatTime = 5, whisperTime = 15 ,redEyeTime = 20, blurTimeModifyer;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    AudioManager audioManager;
    float redEyeGapTime, darkTime;
    float camSize;
    bool camIsShaking;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        playerDie = GetComponent<PlayerDie>();
        blurMaterial.SetFloat("_BlurAmount", 0);
    }

    void Update()
    {
        darkTime = playerDie.timeInDark;

        if(darkTime == 0 || PlayerDie.isDead)
        {
            blurMaterial.SetFloat("_BlurAmount", 0);
            audioManager.heartBeat.Stop();
            audioManager.whisper.Stop();
            if(!camIsShaking && virtualCamera.m_Lens.OrthographicSize != 8)
                StartCoroutine(CameraZoomIEnumerator(virtualCamera.m_Lens.OrthographicSize, 8));

            return;
        }

        camSize = 8 - darkTime / 20;
        if(!camIsShaking)
        {
            virtualCamera.m_Lens.OrthographicSize = camSize;
        }

        if(darkTime >= heartBeatTime)
        {
            if(!audioManager.heartBeat.isPlaying)
            {
                audioManager.heartBeat.pitch = 0.5f + darkTime / 30;
                audioManager.heartBeat.volume = 0.5f + darkTime / 100;
                audioManager.heartBeat.Play();
                StartCoroutine(BlurIEnumerator());
                StartCoroutine(CameraShakeIEnumerator(camSize, camSize - darkTime / 300));
            }
        }

        if(darkTime >= whisperTime)
        {
            audioManager.whisper.volume = darkTime / 100 - 0.15f;
            if(!audioManager.whisper.isPlaying)
            {
                audioManager.whisper.Play();
            }
        }

        if(darkTime >= redEyeTime)
        {
            if(redEyeGapTime <= 0)
            {
                Instantiate(redEye, transform.position + new Vector3(Random.Range(-13, 13), Random.Range(-9, 9), 0), Quaternion.identity);
                Instantiate(redEye, transform.position + new Vector3(Random.Range(-13, 13), Random.Range(-9, 9), 0), Quaternion.identity);
                redEyeGapTime = Mathf.Max(0.1f, 1.5f - darkTime / 30);
            }
            redEyeGapTime -= Time.deltaTime;
        }
    }

    IEnumerator BlurIEnumerator()
    {
        Debug.Log("blur");
        blurMaterial.SetFloat("_BlurAmount", darkTime / 2000);
        yield return new WaitForSeconds(blurTimeModifyer);
        blurMaterial.SetFloat("_BlurAmount", 0);
        
        yield break;
    }

    IEnumerator CameraShakeIEnumerator(float start, float end)
    {
        camIsShaking = true;
        float timer = 0f;
        while(Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - end) > 0.01f)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(start, end, timer);
            timer += Time.deltaTime * 10;
            yield return null;
        }

        timer = 0f;
        while(Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - camSize) > 0.01f)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(end, camSize, timer);
            timer += Time.deltaTime * 10;
            yield return null;
        }

        yield return null;
        camIsShaking = false;
    }

    IEnumerator CameraZoomIEnumerator(float start, float end)
    {
        StopCoroutine("CameraShakeIEnumerator");
        camIsShaking = true;
        float timer = 0f;
        while(Mathf.Abs(virtualCamera.m_Lens.OrthographicSize - end) > 0.01f)
        {
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(start, end, timer);
            timer += Time.deltaTime;
            yield return null;
        }
        yield return null;
        camIsShaking = false;
    }
}
