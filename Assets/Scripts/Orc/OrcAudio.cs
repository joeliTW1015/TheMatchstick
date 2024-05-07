using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcAudio : MonoBehaviour
{
    [SerializeField] AudioSource audioSourceA, audioSourceB;
    [SerializeField] AudioClip shout, attact, die;

    public void ShoutAudio()
    {
        if(audioSourceA.isPlaying)
            return;

        audioSourceA.clip = shout;
        audioSourceA.Play();
    }

    public void AttactAudio()
    {
        if(audioSourceA.isPlaying)
            return;

        audioSourceA.clip = attact;
        audioSourceA.Play();
    }

    public void DieAudio()
    {
        if(audioSourceA.isPlaying)
            return;

        audioSourceA.clip = die;
        audioSourceA.Play();
    }

    public void WalkAudio()
    {
        audioSourceB.Play();
    }
}
