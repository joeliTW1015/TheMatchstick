using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsAudio : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void AudioActive()
    {
        audioSource.Play();
    }
}
